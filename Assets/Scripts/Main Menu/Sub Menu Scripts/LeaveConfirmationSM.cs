using System;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace CGames
{
    public class LeaveConfirmationSM : SubMenu
    {
        [SerializeField] private Button quitButton;

        private Action loseAllCoinsAction;
        private Action loadDataAction;
        private Action loadMainMenuAction;

        [Inject]
        private void Construct(PlayerDataView playerDV, SceneLoader sceneLoader)
        {
            this.loseAllCoinsAction = playerDV.LoseAllCoins;
            this.loadDataAction = playerDV.LoadData;
            this.loadMainMenuAction = delegate
            {
                sceneLoader.LoadMainMenu();
                quitButton.DisableInteractivityWithText();
            };
        }

        protected override void Awake()
        {
            base.Awake();

            quitButton.onClick.AddListener(ReturnToMainMenu);
        }

        public override void PrepareForOpening() => returnButton.Select();
        public override void PrepareForClosing() { }

        private void ReturnToMainMenu()
        {   
            loseAllCoinsAction?.Invoke();
            loadDataAction?.Invoke();
            
            loadMainMenuAction?.Invoke();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            quitButton.onClick.RemoveListener(ReturnToMainMenu);
        }
    }
}