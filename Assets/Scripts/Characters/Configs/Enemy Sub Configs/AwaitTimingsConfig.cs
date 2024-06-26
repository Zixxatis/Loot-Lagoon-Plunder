using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class AwaitTimingsConfig : ICloneable<AwaitTimingsConfig>    
    {
        [field: SerializeField] public AwaitTimingType AwaitTimingType { get; private set; }
        [field: SerializeField, Min(0.01f)] public float AwaitInSeconds { get; private set; }

        public AwaitTimingsConfig Clone()
        {
            return new AwaitTimingsConfig
            {
                AwaitTimingType = this.AwaitTimingType,
                AwaitInSeconds = this.AwaitInSeconds
            };
        }
    }

    public enum AwaitTimingType
    {
        GeneralAwait,
        PreStrikeAwait,
        AfterAttackRecovery
    }
}