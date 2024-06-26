using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class CharacterAttributes
    {
        [field: SerializeField] public HealthSystemConfig HealthSystemConfig { get; private set; }
        [field: Space]
        [field: SerializeField] public MovementConfig MovementConfig { get; private set; }
        [field: SerializeField] public AirborneConfig AirborneConfig { get; private set; }
        [field: Space]
        [field: SerializeField] protected List<AttackConfig> AttackConfigsList { get; private set; } = new();

        public CharacterAttributes(CharacterConfig baseCharacterConfig)
        {
            HealthSystemConfig = baseCharacterConfig.HealthSystemConfig.Clone();
            MovementConfig = baseCharacterConfig.MovementConfig.Clone();
            AirborneConfig = baseCharacterConfig.AirborneConfig.Clone();
            baseCharacterConfig.AttackConfigsList.ForEach(x => AttackConfigsList.Add(x.Clone()));
        }

        public AttackConfig GetAttackConfig(AttackType attackType) => AttackConfigsList.Find(x => x.AttackType == attackType);
    }
}