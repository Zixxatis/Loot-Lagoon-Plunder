using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class HealthSystemConfig : ICloneable<HealthSystemConfig>
    {
        [field: SerializeField, Min(10)] public int MaxHealth { get; private set; }
        [field: SerializeField] public float RecoveryWindowInSeconds { get; private set; }
        [field: Space]
        [field: SerializeField] public bool HasResistToKnockBack { get; private set; }
        [field: SerializeField] public bool IsKnockable { get; private set; }
        [field: SerializeField] public bool IsCountedForVampiric { get; private set; } = true;

        public HealthSystemConfig Clone()
        {
            return new HealthSystemConfig
            {
                MaxHealth = this.MaxHealth,
                RecoveryWindowInSeconds = this.RecoveryWindowInSeconds,
                HasResistToKnockBack = this.HasResistToKnockBack,
                IsKnockable = this.IsKnockable,
                IsCountedForVampiric = this.IsCountedForVampiric
            };
        }

        public void IncreaseMaxHealth(int amountToIncrease) => MaxHealth += amountToIncrease;

        public void DisableVampiricAffect() => IsCountedForVampiric = false;
    }
}
