using System;
using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public abstract class MerchantPanel : MonoBehaviour
    {
        [Header("Blocker")]
        [SerializeField] private Image blockerImage;

        [Header("UI Elements")]
        [SerializeField] protected Button returnButton;

        private Action returnAction;

        public void Initialize(Action returnAction)
        {
            this.returnAction = returnAction;

            returnButton.onClick.AddListener(Return);

            InitializePanel();
            HidePanel();
        }

        protected abstract void InitializePanel();

        public virtual void ShowPanel()
        {
            this.ActivateGameObject();
            blockerImage.ActivateGameObject();
        }

        protected virtual void HidePanel()
        {
            blockerImage.DeactivateGameObject();
            this.DeactivateGameObject();
        }

        private void Return()
        {
            returnAction?.Invoke();
            HidePanel();
        }

        protected virtual void OnDestroy() => returnButton.onClick.RemoveListener(Return);
    }
}