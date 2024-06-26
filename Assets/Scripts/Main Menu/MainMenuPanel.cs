using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class MainMenuPanel : MonoBehaviour
    {
        [Header("Sub Menus")]
        [SerializeField] private SettingsSM settingsSM;
        [SerializeField] private ControlsSM controlsSM;
        [SerializeField] private RulesSM rulesSM;

        [Header("Navigation Buttons")]
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button controlsButton;
        [SerializeField] private Button rulesButton;
        [SerializeField] private Button exitButton;

        [Header("Various")]
        [SerializeField] private Image blockerImage;
        [Space]
        [SerializeField] private TextMeshProUGUI gameVersion;

        private Action loadShipAction;
        private List<SubMenu> subMenuList;

        [Inject]
        private void Construct(SceneLoader sceneLoader)
        {
            this.loadShipAction = delegate
            {
                sceneLoader.LoadShip();
                playButton.DisableInteractivityWithText();
            };
        }
        
        private void Awake()
        {
            playButton.onClick.AddListener(loadShipAction.Invoke);
            settingsButton.onClick.AddListener(settingsSM.OpenSubMenu);
            controlsButton.onClick.AddListener(controlsSM.OpenSubMenu);
            rulesButton.onClick.AddListener(rulesSM.OpenSubMenu);
            exitButton.onClick.AddListener(Application.Quit);

            subMenuList = new()
            {
                settingsSM,
                controlsSM,
                rulesSM
            };

            gameVersion.text = Application.version;

            blockerImage.DeactivateGameObject();
        }

        private void Start()
        {
            PlayerInputHandler.DisableInputActions();

            foreach (SubMenu subMenu in subMenuList)
            {
                subMenu.OnSubMenuOpened += blockerImage.ActivateGameObject;
                subMenu.OnSubMenuOpened += DisableAllButtons;

                subMenu.OnSubMenuClosed += blockerImage.DeactivateGameObject;
                subMenu.OnSubMenuClosed += EnableAllButtons;
                subMenu.OnSubMenuClosed += () => SelectLastButton(subMenu);
            }
            
            playButton.Select();
        }

        private void SelectLastButton(SubMenu subMenu) 
        {
            switch (subMenu)
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
                    
                default:
                    throw new NotSupportedException();
            }
        }

        private void EnableAllButtons()
        {
            playButton.interactable = true;
            settingsButton.interactable = true;
            controlsButton.interactable = true;
            rulesButton.interactable = true;
            exitButton.interactable = true;
        }

        private void DisableAllButtons()
        {
            playButton.interactable = false;
            settingsButton.interactable = false;
            controlsButton.interactable = false;
            rulesButton.interactable = false;
            exitButton.interactable = false;
        }   
        
        private void OnDestroy()
        {
            playButton.onClick.RemoveListener(loadShipAction.Invoke);
            settingsButton.onClick.RemoveListener(settingsSM.OpenSubMenu);
            controlsButton.onClick.RemoveListener(controlsSM.OpenSubMenu);
            rulesButton.onClick.RemoveListener(rulesSM.OpenSubMenu);
            exitButton.onClick.RemoveListener(Application.Quit);

            foreach (SubMenu subMenu in subMenuList)
            {
                subMenu.OnSubMenuOpened -= blockerImage.ActivateGameObject;
                subMenu.OnSubMenuOpened -= DisableAllButtons;

                subMenu.OnSubMenuClosed -= blockerImage.DeactivateGameObject;
                subMenu.OnSubMenuClosed -= EnableAllButtons;
                subMenu.OnSubMenuClosed -= () => SelectLastButton(subMenu);
            }
        }
    }
}