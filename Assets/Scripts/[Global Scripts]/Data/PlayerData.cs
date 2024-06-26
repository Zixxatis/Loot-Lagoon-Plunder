using System;
using System.Collections.Generic;

namespace CGames
{
    [Serializable]
    public class PlayerData
    {
        public const byte PlayerFileVersion = 1; 

        // Global
        public byte SaveVersion { get; set; }

        // Player's Progress
        public ColoredGems ColoredGems { get; set; }
        public Dictionary<PlayerUpgradeType, bool> UpgradesDictionary { get; set; }

        public PlayerData()
        {        
            SaveVersion = PlayerFileVersion;

            ColoredGems = new(0, 0, 0);
            UpgradesDictionary = new();
        }
    }
}