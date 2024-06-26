using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class TouchKnockBackConfig : ICloneable<TouchKnockBackConfig>
    {
        [field: SerializeField, Min(0)] public int Damage { get; private set; }
        [field: Space]
        [field: SerializeField] public Vector2 KnockBackForce { get; private set; }

        public TouchKnockBackConfig Clone()
        {
            return new TouchKnockBackConfig
            {
                Damage = this.Damage,
                KnockBackForce = this.KnockBackForce
            };
        }
    }
}