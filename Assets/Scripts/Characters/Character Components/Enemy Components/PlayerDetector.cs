using System;
using CGames.Utilities;
using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Collider2D))]
    public class PlayerDetector : MonoBehaviour
    {
        public event Action OnEnemyDetected;
        public event Action OnEnemyLost;

        public Character DetectedCharacter { get; private set; }
        public bool IsTargetInRange { get; private set; }

        public void Initialize(float detectionRadius)
        {
            if(GetComponent<Collider2D>() is CircleCollider2D circleCollider2D)
                circleCollider2D.radius = detectionRadius;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (DetectedCharacter != null)
                return;

            if (other.CompareTag(LayerUtilities.PlayerTag) == false)
                return;
            
            SetTarget(other);
        }

        private void SetTarget(Collider2D other)
        {
            IsTargetInRange = true;
            DetectedCharacter = other.transform.GetComponentInParent<Character>();

            OnEnemyDetected?.Invoke();
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (DetectedCharacter == null)
                return;

            if (other.CompareTag(LayerUtilities.PlayerTag) == false)
                return;
                
            LoseTarget();
        }

        public void LoseTarget()
        {
            IsTargetInRange = false;
            DetectedCharacter = null;

            OnEnemyLost?.Invoke();
        }
    }
}