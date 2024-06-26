using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Zenject;

namespace CGames
{
    public class SettingsSM : SubMenu
    {
        [Header("Navigation")]
        [SerializeField] private Button leftButton;
        [SerializeField] private Button rightButton;
        [Space]
        [SerializeField] private GameObject generalSettingsObject;
        [SerializeField] private GameObject pcSettingsObject;

        [Header("General Settings")]
        [SerializeField] private TMP_Dropdown localeDropdown;
        [Space]
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private TextMeshProUGUI musicVolumeTMP;
        [Space]
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private TextMeshProUGUI sfxVolumeTMP;

        [Header("PC Settings")]
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [Space]
        [SerializeField] private Toggle fullScreenModeToggle;
        [SerializeField] private GameObject fullScreenNotificationObject;
        [Space]
        [SerializeField] private Toggle cursorToggle;

        [Header("Misc")]
        [SerializeField, Min(0f)] private float dropdownElementHeight = 50f;
        [SerializeField, Min(0f)] private float dropdownElementsSpacing = 5f;
        
        private ConfigDataView configDV;
        private List<KeyValuePair<string, Vector2>> supportedScreenSizes;

        [Inject]
        private void Construct(ConfigDataView configDV)
        {
            this.configDV = configDV;
        }

        protected override void Awake()
        {
            base.Awake();

            PrepareNavigationButtons();
            PrepareLocalizationSettings();
            PrepareAudioSettings();
            PrepareResolutionSettings();
            PrepareScreenModeSettings();
            PrepareCursorSettings();

            DisplayGeneralSettings();
        }

    #region Preparations
        private void PrepareNavigationButtons()
        {
            leftButton.onClick.AddListener(DisplayGeneralSettings);
            rightButton.onClick.AddListener(DisplayPCSettings);
        }

        private void PrepareLocalizationSettings()
        {
            foreach(Language language in Enum.GetValues(typeof(Language)))
            {   
                string displayedLanguage = language switch
                {
                    Language.English => "English",
                    Language.Russian => "Русский",
                    _ => string.Empty,
                };

                if(string.IsNullOrEmpty(displayedLanguage))
                    Debug.LogError("Tried to add unsupported language from enum to dropdown. Won't add it.");
                else
                    localeDropdown.options.Add(new TMP_Dropdown.OptionData(displayedLanguage));
            }

            RectTransform templateRectTransform = localeDropdown.template.GetComponent<RectTransform>();
            float templateHeight = localeDropdown.options.Count * (dropdownElementHeight + dropdownElementsSpacing) + dropdownElementsSpacing;
            templateRectTransform.sizeDelta = new Vector2(templateRectTransform.sizeDelta.x, templateHeight);

            localeDropdown.onValueChanged.AddListener(configDV.ChangeLanguage);
        }

        private void PrepareAudioSettings()
        {
            musicVolumeSlider.minValue = sfxVolumeSlider.minValue = 0;
            musicVolumeSlider.maxValue = sfxVolumeSlider.maxValue = 1;
            musicVolumeSlider.wholeNumbers = sfxVolumeSlider.wholeNumbers = false;

            musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
        }

        private void PrepareResolutionSettings()
        {
            supportedScreenSizes = new()
            {
                new ("UHD (4K)", new(3840, 2160)),
                new ("QHD (2K)", new(2560, 1440)),
                new ("Full HD", new(1920, 1080)),
                new ("HD+", new(1600, 900)),
                new ("HD", new(1280, 720)),
                new ("Half FHD", new(960, 540)),
                new ("Half HD+", new(800, 450)),
                new ("Half HD", new(640, 360)),
            };

            // ? Removes all entries that are too big for current screen.
            supportedScreenSizes.RemoveAll(x => x.Value.x > Screen.currentResolution.width);
            supportedScreenSizes.ForEach(x => resolutionDropdown.options.Add(new TMP_Dropdown.OptionData($"{x.Key} - {x.Value.x}x{x.Value.y}")));

            RectTransform templateRectTransform = resolutionDropdown.template.GetComponent<RectTransform>();
            float templateHeight = resolutionDropdown.options.Count * (dropdownElementHeight + dropdownElementsSpacing) + dropdownElementsSpacing;
            templateRectTransform.sizeDelta = new Vector2(templateRectTransform.sizeDelta.x, templateHeight);

            resolutionDropdown.onValueChanged.AddListener(ChangeResolutionFromPresetIndex);
        }

        private void PrepareScreenModeSettings()
        {  
            bool isMonitor16x9 = Screen.currentResolution.width / 16 == Screen.currentResolution.height / 9;

            if(isMonitor16x9)
            {
                fullScreenModeToggle.onValueChanged.AddListener(ChangeScreenMode);
                fullScreenNotificationObject.DeactivateObject();
            }
            else
                fullScreenModeToggle.DeactivateGameObject();
        }

        private void PrepareCursorSettings()
        {
            cursorToggle.isOn = configDV.ShouldDisplayCursor;

            cursorToggle.onValueChanged.AddListener(configDV.ChangeCursorVisibility);
        }
    #endregion

    #region Pre-displaying
        public override void PrepareForOpening()
        {
            MatchLocalizationSettings();
            MatchSoundSettings();
            MatchResolution();
            MatchScreenMode();

            DisplayGeneralSettings();
        }

        private void MatchLocalizationSettings()
        {
            localeDropdown.value = (int)configDV.Language;
            localeDropdown.RefreshShownValue();
        }

        private void MatchSoundSettings()
        {
            musicVolumeSlider.value = configDV.MusicVolume;
            sfxVolumeSlider.value = configDV.EffectsVolume;

            musicVolumeTMP.text = $"{configDV.MusicVolume * 100}";
            sfxVolumeTMP.text = $"{configDV.EffectsVolume * 100}";
        }

        private void MatchResolution()
        {
            resolutionDropdown.value = supportedScreenSizes.IndexOf(supportedScreenSizes.First(x => x.Value.x <= Screen.width));
            resolutionDropdown.RefreshShownValue();

            resolutionDropdown.interactable = Screen.fullScreenMode != FullScreenMode.FullScreenWindow;
        }

        private void MatchScreenMode()
        {
            fullScreenModeToggle.isOn = Screen.fullScreenMode == FullScreenMode.FullScreenWindow;
        }
    #endregion

    #region Displaying Behaviour
        private void DisplayGeneralSettings()
        {
            leftButton.interactable = false;
            rightButton.interactable = true;

            generalSettingsObject.ActivateObject();
            pcSettingsObject.DeactivateObject();

            localeDropdown.Select();
        }

        private void DisplayPCSettings()
        {
            leftButton.interactable = true;
            rightButton.interactable = false;

            generalSettingsObject.DeactivateObject();
            pcSettingsObject.ActivateObject();

            resolutionDropdown.Select();
        }
    #endregion

    #region Changes
        private void ChangeMusicVolume(float value)
        {   
            float volumeValue = (float)Math.Round(value, 2);
            
            musicVolumeTMP.text = $"{volumeValue * 100}";

            configDV.ChangeMusicVolume(volumeValue);
        }

        private void ChangeSFXVolume(float value)
        {   
            float volumeValue = (float)Math.Round(value, 2);

            sfxVolumeTMP.text = $"{volumeValue * 100}";

            configDV.ChangeEffectsVolume(volumeValue);
        }

        private void ChangeScreenMode(bool shouldBeFullScreen)
        {
            if (shouldBeFullScreen)
            {   
                // ? Will force to display MAX possible resolution.
                resolutionDropdown.value = 0;
                resolutionDropdown.RefreshShownValue();
                resolutionDropdown.interactable = false;

                // ? Sets Full Screen mode with MAX possible resolution.
                Screen.SetResolution((int)supportedScreenSizes[0].Value.x, (int)supportedScreenSizes[0].Value.y, FullScreenMode.FullScreenWindow);
            }
            else
            {
                resolutionDropdown.interactable = true;
                
                KeyValuePair<string, Vector2> currentScreenSizes = supportedScreenSizes.First(x => x.Value.x <= Screen.width);
                Screen.SetResolution((int)currentScreenSizes.Value.x, (int)currentScreenSizes.Value.y, FullScreenMode.Windowed);
            }
        }

        private void ChangeResolutionFromPresetIndex(int index)
        {   
            Screen.SetResolution((int)supportedScreenSizes[index].Value.x, (int)supportedScreenSizes[index].Value.y, Screen.fullScreenMode);
        }

    #endregion

        public override void PrepareForClosing() => configDV.SaveData();

        protected override void OnDestroy()
        {
            base.OnDestroy();

            leftButton.onClick.RemoveListener(DisplayGeneralSettings);
            rightButton.onClick.RemoveListener(DisplayPCSettings);
        }
    }
}