using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class EnemyAttributes : CharacterAttributes
    {
        [field: Header("Enemy Config")]
        [field: SerializeField] public TouchKnockBackConfig TouchKnockBackConfig { get; private set; }
        [field: SerializeField] public DetectionConfig DetectionConfig { get; private set; }
        [field: SerializeField] public CoinRewardConfig CoinRewardConfig { get; private set; }
        [Space]
        [SerializeField] private List<AwaitTimingsConfig> awaitTimingsConfigList = new();

        public EnemyAttributes(EnemyConfig baseEnemyConfig) : base(baseEnemyConfig)
        {
            TouchKnockBackConfig = baseEnemyConfig.TouchKnockBackConfig.Clone();
            DetectionConfig = baseEnemyConfig.DetectionConfig.Clone();
            CoinRewardConfig = baseEnemyConfig.CoinRewardConfig.Clone();
            baseEnemyConfig.AwaitTimingsConfigList.ForEach(x => awaitTimingsConfigList.Add(x.Clone()));
        }

        public float GetAwaitTime(AwaitTimingType awaitTimingType) => awaitTimingsConfigList.Find(x => x.AwaitTimingType == awaitTimingType).AwaitInSeconds;
    }
}