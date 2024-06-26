using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public class ControlsSM : SubMenu
    {   
        [Header("Controls Panel - Elements")]
        [SerializeField] private GameObject keyboardControlsObject;
        [SerializeField] private GameObject gamepadControlsObject;
        [Space]
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;

        protected override void Awake()
        {
            base.Awake();

            leftButton.onClick.AddListener(ShowKeyBoardControls);
            rightButton.onClick.AddListener(ShowGamepadControls);
        }

        public override void PrepareForOpening() => ShowKeyBoardControls();

        private void ShowKeyBoardControls()
        {
            leftButton.interactable = false;
            rightButton.interactable = true;

            keyboardControlsObject.ActivateObject();
            gamepadControlsObject.DeactivateObject();

            rightButton.Select();
        }

        private void ShowGamepadControls()
        {
            leftButton.interactable = true;
            rightButton.interactable = false;

            keyboardControlsObject.DeactivateObject();
            gamepadControlsObject.ActivateObject();

            leftButton.Select();
        }

        public override void PrepareForClosing() { }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            leftButton.onClick.RemoveListener(ShowKeyBoardControls);
            rightButton.onClick.RemoveListener(ShowGamepadControls);
        }
    }
}