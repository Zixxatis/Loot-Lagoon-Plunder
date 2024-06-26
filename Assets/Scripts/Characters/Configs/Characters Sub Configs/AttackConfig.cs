using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class AttackConfig : ICloneable<AttackConfig>
    {
        [field: SerializeField] public AttackType AttackType { get; private set; }
        [field: Space]
        [field: SerializeField] public float AttackRangeRadius { get; private set; }
        [field: SerializeField, Min(0)] public int Damage { get; private set; }
        [field: Space]
        [field: SerializeField, Min(0.01f)] public float DurationInSeconds { get; private set; }
        [field: SerializeField, Min(0.01f)] public float DelayBeforeDamagingInSeconds { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 KnockBackForce { get; private set; }
        [field: Space]
        [field: SerializeField, Range(0, 100)] public int CriticalChanceInPercent { get; private set; }
        [field: Space]
        [field: SerializeField] public int VampiricHealAmount { get; private set; }

        /// <summary> Combined time of attack delay & attack duration </summary>
        public float CombinedAttackTimeInSeconds => DelayBeforeDamagingInSeconds + DurationInSeconds;

        public AttackConfig Clone()
        {
            return new AttackConfig
            {
                AttackType = this.AttackType,
                AttackRangeRadius = this.AttackRangeRadius,
                Damage = this.Damage,
                DurationInSeconds = this.DurationInSeconds,
                DelayBeforeDamagingInSeconds = this.DelayBeforeDamagingInSeconds,
                KnockBackForce = this.KnockBackForce,
                CriticalChanceInPercent = this.CriticalChanceInPercent,
                VampiricHealAmount = this.VampiricHealAmount
            };
        }

        public void IncreaseDamage(int damageUpgradeAmount) => Damage += damageUpgradeAmount;
        public void SetCriticalChange(int criticalChanceInPercent) => CriticalChanceInPercent = criticalChanceInPercent;
        public void SetVampiricAmount(int vampiricHealAmount) => VampiricHealAmount = vampiricHealAmount;
    }

    public enum AttackType
    {
        Default,
        Airborne,
        Alternative
    }
}