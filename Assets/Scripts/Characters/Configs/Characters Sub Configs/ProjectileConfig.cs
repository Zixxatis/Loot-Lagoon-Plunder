using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class ProjectileConfig : ICloneable<ProjectileConfig>
    {
        [field: SerializeField] public float ProjectileSpeed { get; private set; }
        [field: Space]
        [field: SerializeField] public int Damage { get; private set; }
        [field: SerializeField] public Vector2 KnockBackForce { get; private set; }
        [field: Space]
        [field: SerializeField] public float PreDestructionAwaitDuration { get; private set; }

        public ProjectileConfig Clone()
        {
            return new ProjectileConfig
            {
                ProjectileSpeed = this.ProjectileSpeed,
                Damage = this.Damage,
                KnockBackForce = this.KnockBackForce,
                PreDestructionAwaitDuration = this.PreDestructionAwaitDuration
            };
        }
    }
}