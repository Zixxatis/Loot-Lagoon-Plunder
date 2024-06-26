using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class MovementConfig : ICloneable<MovementConfig>
    {
        [field: SerializeField] public float MaxSpeedX { get; private set; }
        [field: Space]
        [field: SerializeField] public float Acceleration { get; private set; }
        [field:SerializeField, Range(0.5f, 1f)] public float InertiaMultiplier { get; private set; }
        [field: Space]
        [field: SerializeField, Min(0.005f)] public float MinimumVelocityToCountAsMovement { get; private set; }

        public MovementConfig Clone()
        {
            return new MovementConfig
            {
                MaxSpeedX = this.MaxSpeedX,
                Acceleration = this.Acceleration,
                InertiaMultiplier = this.InertiaMultiplier,
                MinimumVelocityToCountAsMovement = this.MinimumVelocityToCountAsMovement
            };
        }

        public void IncreaseMoveSpeed(float moveSpeedUpgradeIncrease) => MaxSpeedX += moveSpeedUpgradeIncrease;
    }
}