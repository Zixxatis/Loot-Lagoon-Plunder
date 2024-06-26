using System;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class DetectionConfig : ICloneable<DetectionConfig>   
    {
        [field: SerializeField, Min(0.1f)] public float LongDetectionRadius { get; private set; }
        [field: SerializeField, Min(0.1f)] public float CloseDetectionRadius { get; private set; }
        [field: Space]
        [field: SerializeField] public bool ShouldUseDefaultDialogueBehaviour { get; private set; } = true;

        public DetectionConfig Clone()
        {
            return new DetectionConfig
            {
                LongDetectionRadius = this.LongDetectionRadius,
                CloseDetectionRadius = this.CloseDetectionRadius,
                ShouldUseDefaultDialogueBehaviour = this.ShouldUseDefaultDialogueBehaviour
            };
        }
    }
}