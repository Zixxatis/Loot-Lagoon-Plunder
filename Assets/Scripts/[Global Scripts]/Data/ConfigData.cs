using System;

namespace CGames
{
    [Serializable]
    public class ConfigData
    {
        public const byte ConfigFileVersion = 1; 
        public static bool IsDeveloperMode = false;
        
        public byte SaveVersion { get; set; }

        // Sound Settings
        public float MusicVolume { get; set; }
        public float EffectsVolume { get; set; }

        // Locale Settings
        public Language Language { get; set; }

        // Cursor Settings
        public bool ShouldDisplayCursor { get; set; }


        /// <summary> Values in this constructor will be set as default when creating a new save file. </summary>
        public ConfigData()
        {
            SaveVersion = ConfigFileVersion;

            MusicVolume = 0.4f;
            EffectsVolume = 0.6f;

            Language = GetSystemLanguage();

            ShouldDisplayCursor = true;
        }

        private Language GetSystemLanguage()
        {
            string languageCode = System.Globalization.CultureInfo.CurrentCulture.ToString()[..2];

            return languageCode switch
            {
                "ru" or "be" or "hy" or "ky" or "kk" or "ua" => Language.Russian,
                _ => Language.English
            };
        }
    }
}