using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [CreateAssetMenu(fileName = "Character Config", menuName = "Scriptable Objects/Configs/Enemy Config", order = 1)]
    public class EnemyConfig: CharacterConfig
    {
        [field: Header("Enemy Config")]
        [field: SerializeField] public TouchKnockBackConfig TouchKnockBackConfig { get; private set; }
        [field: SerializeField] public DetectionConfig DetectionConfig { get; private set; }
        [field: SerializeField] public CoinRewardConfig CoinRewardConfig { get; private set; }
        [field:Space]
        [field: SerializeField] public List<AwaitTimingsConfig> AwaitTimingsConfigList { get; private set; }
    }
}