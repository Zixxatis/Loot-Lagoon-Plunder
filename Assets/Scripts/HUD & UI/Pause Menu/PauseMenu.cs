using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace CGames
{
    public class PauseMenu : MonoBehaviour
    {
        [Header("Sub Menus")]
        [SerializeField] private SettingsSM settingsSM;
        [SerializeField] private ControlsSM controlsSM;
        [SerializeField] private RulesSM rulesSM;
        [SerializeField] private LeaveConfirmationSM leaveConfirmationSM;

        [Header("Buttons")]
        [SerializeField] private Button returnButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button rulesButton;
        [SerializeField] private Button quitButton;

        [Header("Blocker")]
        [SerializeField] private Image blockerImage;

        private List<SubMenu> subMenuList;
        private SubMenu openedSubMenu;

        private void Awake()
        {
            PlayerInputHandler.OnSwappedToEnablePlayerScheme += ExitFromPause;
            PlayerInputHandler.OnSwappedToDisablePlayerScheme += EnterPause;

            Initialize();
            Deactivate();
        }

        private void Initialize()
        {
            returnButton.onClick.AddListener(PlayerInputHandler.ChangePlayerControlsStatus);
            settingsButton.onClick.AddListener(() => OpenSubMenu(settingsSM));
            controlsButton.onClick.AddListener(() => OpenSubMenu(controlsSM));
            rulesButton.onClick.AddListener(() => OpenSubMenu(rulesSM));
            quitButton.onClick.AddListener(() => OpenSubMenu(leaveConfirmationSM));

            subMenuList = new()
            {
                settingsSM,
                controlsSM,
                rulesSM,
                leaveConfirmationSM
            };

            foreach (SubMenu subMenu in subMenuList)
            {
                subMenu.OnSubMenuOpened += DisableAllButtons;

                subMenu.OnSubMenuClosed += EnableAllButtons;
                subMenu.OnSubMenuClosed += ResetOpenedSubMenuReference;
                subMenu.OnSubMenuClosed += () => SelectLastButton(subMenu);
            }
        }

        private void Activate()
        {
            this.ActivateGameObject();
            blockerImage.ActivateGameObject();

            returnButton.Select();
        }

        private void Deactivate()
        {
            if(openedSubMenu != null)
                openedSubMenu.CloseSubMenu();

            blockerImage.DeactivateGameObject();
            this.DeactivateGameObject();
        }

        private void OpenSubMenu(SubMenu subMenu)
        {
            this.openedSubMenu = subMenu;
            subMenu.OpenSubMenu();
        }

        private void ResetOpenedSubMenuReference() => openedSubMenu = null;

        private void EnableAllButtons()
        {
            returnButton.interactable = true;
            settingsButton.interactable = true;
            controlsButton.interactable = true;
            rulesButton.interactable = true;
            quitButton.interactable = true;
        }

        private void DisableAllButtons()
        {
            returnButton.interactable = false;
            settingsButton.interactable = false;
            controlsButton.interactable = false;
            rulesButton.interactable = false;
            quitButton.interactable = false;
        }  

        private void SelectLastButton(SubMenu subMenu) 
        {
            switch(subMenu)
            {
                case SettingsSM:
                    settingsButton.Select();
                    break;

                case ControlsSM:
                    controlsButton.Select();
                    break;

                case RulesSM:
                    rulesButton.Select();
                    break;
                    
                case LeaveConfirmationSM:
                    quitButton.Select();
                    break;
            }
        }

        private void EnterPause()
        {
            Activate();
            Time.timeScale = 0;

            PlayerInputHandler.DisplayCursor();
        }

        private void ExitFromPause()
        {
            Deactivate();
            Time.timeScale = 1;

            PlayerInputHandler.MatchCursorPreference();
        }

        private void OnDestroy()
        {
            if(Time.timeScale != 1)
                Time.timeScale = 1;

            PlayerInputHandler.OnSwappedToEnablePlayerScheme -= ExitFromPause; 
            PlayerInputHandler.OnSwappedToDisablePlayerScheme -= EnterPause;

            returnButton.onClick.RemoveListener(PlayerInputHandler.ChangePlayerControlsStatus);
            settingsButton.onClick.RemoveListener(() => OpenSubMenu(settingsSM));
            controlsButton.onClick.RemoveListener(() => OpenSubMenu(controlsSM));
            rulesButton.onClick.RemoveListener(() => OpenSubMenu(rulesSM));
            quitButton.onClick.RemoveListener(() => OpenSubMenu(leaveConfirmationSM));

            foreach (SubMenu subMenu in subMenuList)
            {
                subMenu.OnSubMenuOpened -= DisableAllButtons;

                subMenu.OnSubMenuClosed -= EnableAllButtons;
                subMenu.OnSubMenuClosed -= ResetOpenedSubMenuReference;
                subMenu.OnSubMenuClosed -= () => SelectLastButton(subMenu);
            }
        }
    }
}