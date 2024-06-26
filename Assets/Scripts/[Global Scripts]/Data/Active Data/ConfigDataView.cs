using System;

namespace CGames
{
    public class ConfigDataView : DataView<ConfigData>
    {
        public byte SaveVersion { get ; private set; }

        // Sound Settings
        public float MusicVolume { get ; private set; }
        public float EffectsVolume { get ; private set; }

        // Locale Settings
        public Language Language { get ; private set; }

        // Cursor Settings
        public bool ShouldDisplayCursor { get ; private set; }
        
        protected override string LogPrefix => "CONFIG"; 
        protected override string FileName => "config.data";
        protected override ConfigData NewData => new();

        public event Action OnVolumeChanged;
        public event Action OnLocalizationChanged;
        
        public ConfigDataView(MonoProxy monoProxy, ResourceSystem resourceSystem) : base(monoProxy, resourceSystem) { }

        protected override void ApplyData(ConfigData data)
        {
            SaveVersion = data.SaveVersion;

            MusicVolume = data.MusicVolume;
            EffectsVolume = data.EffectsVolume;

            Language = data.Language;

            ShouldDisplayCursor = data.ShouldDisplayCursor;
        }

        protected override ConfigData ReadAllData()
        {
            return new()
            {
                SaveVersion = this.SaveVersion,

                MusicVolume = this.MusicVolume,
                EffectsVolume = this.EffectsVolume,

                Language = this.Language,

                ShouldDisplayCursor = this.ShouldDisplayCursor
            };
        }

        public void ChangeLanguage(int languageIndex) => ChangeLanguage((Language)languageIndex);
        public void ChangeLanguage(Language language)
        {
            if(Language == language)
                return;

            Language = language;
            OnLocalizationChanged?.Invoke();
        }
        
        public void ChangeMusicVolume(float newVolume)
        {
            MusicVolume = newVolume;
            OnVolumeChanged?.Invoke();
        }
        public void ChangeEffectsVolume(float newVolume) => EffectsVolume = newVolume;

        public void ChangeCursorVisibility(bool shouldDisplayCursor) => this.ShouldDisplayCursor = shouldDisplayCursor;
    }
}