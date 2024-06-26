using System;
using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public abstract class SubMenu : MonoBehaviour
    {
        [Header("Sub Menu Elements")]
        [SerializeField] protected Button returnButton;

        protected virtual void Awake() => returnButton.onClick.AddListener(CloseSubMenu);
        protected virtual void Start() => this.DeactivateGameObject();

        public event Action OnSubMenuOpened;
        public event Action OnSubMenuClosed;

        public void OpenSubMenu()
        {
            PrepareForOpening();
            this.ActivateGameObject();
            OnSubMenuOpened?.Invoke();
        }
        public abstract void PrepareForOpening();

        public void CloseSubMenu()
        {
            PrepareForClosing();
            this.DeactivateGameObject();
            OnSubMenuClosed?.Invoke();
        }
        public abstract void PrepareForClosing();

        protected virtual void OnDestroy() => returnButton.onClick.RemoveListener(CloseSubMenu);
    }
}