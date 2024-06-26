using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace CGames
{
    public class PlayerDataView : DataView<PlayerData>
    {
        public byte SaveVersion { get ; private set; }

        // Player's Progress
        public ColoredGems ColoredGems { get ; private set; }
        public Dictionary<PlayerUpgradeType, bool> UpgradesDictionary { get; private set; }

        // Non-Persistent
        public int CoinsAmount { get; private set; }
        public int HealthAtTheEndOfLevel { get; private set; }

        protected override string LogPrefix => "GAME";
        protected override string FileName => "player.data";
        protected override PlayerData NewData => new();

        public PlayerDataView(MonoProxy monoProxy, ResourceSystem resourceSystem) : base(monoProxy, resourceSystem) { }

        protected override void ApplyData(PlayerData data)
        {
            SaveVersion = data.SaveVersion;

            ColoredGems = data.ColoredGems;

            UpgradesDictionary = GetCorrectPlayerUpgradesDictionary(data.UpgradesDictionary);
        }

        protected override PlayerData ReadAllData()
        {
            return new()
            {
                SaveVersion = this.SaveVersion,

                ColoredGems = this.ColoredGems,
                UpgradesDictionary = this.UpgradesDictionary
            };
        }

        private Dictionary<PlayerUpgradeType, bool> GetCorrectPlayerUpgradesDictionary(Dictionary<PlayerUpgradeType, bool> dictionaryFromSaves)
        {
            Dictionary<PlayerUpgradeType, bool> newDictionary = new();
            resourceSystem.PlayerUpgradesList.ForEach(x => newDictionary.Add(x.UpgradeType, false));
            
            if (newDictionary.Count == dictionaryFromSaves.Count) 
                return dictionaryFromSaves;

            if(newDictionary.Count > dictionaryFromSaves.Count)
            {
                for (int i = 0; i < dictionaryFromSaves.Count; i++)
                {
                    newDictionary[(PlayerUpgradeType)i] = dictionaryFromSaves[(PlayerUpgradeType)i];
                }
            }

            return newDictionary;
        }

        public void SaveHealthAmountAtTheEndOfLevel() => HealthAtTheEndOfLevel = Object.FindObjectOfType<PlayerCharacter>().HealthSystem.Health;

        public void GetGems(ColoredGems gemsAmount) => ColoredGems += gemsAmount;
        public void SpendGems(ColoredGems gemsAmount) => ColoredGems -= gemsAmount;

        public void CollectCoin(int coinReward)
        {
            if(UpgradesDictionary[PlayerUpgradeType.Midas] == true)
                coinReward ++;

            CoinsAmount += Mathf.Abs(coinReward);
        }

        public void SpendCoin(int coinReward) => CoinsAmount -= Mathf.Abs(coinReward);
        public void LoseAllCoins() => CoinsAmount = 0;

        public void UnlockUpgrade(PlayerUpgradeType upgradeType) => UpgradesDictionary[upgradeType] = true;
        public bool HasAllUpgradesUnlocked() => UpgradesDictionary.Values.All(x => x == true);
        public bool HasAllUpgradesLocked() => UpgradesDictionary.Values.All(x => x == false);
    }
}