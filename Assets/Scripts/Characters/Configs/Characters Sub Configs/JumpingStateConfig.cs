using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class AirborneConfig : ICloneable<AirborneConfig>
    {
        [field: SerializeField] public float JumpForce { get; private set; }

        public AirborneConfig Clone()
        {
            return new AirborneConfig
            {
                JumpForce = this.JumpForce
            };
        }

        public void IncreaseJumpForce(float jumpForceUpgradeIncrease) => JumpForce += jumpForceUpgradeIncrease;
    }
}