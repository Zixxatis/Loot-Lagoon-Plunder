using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CGames
{
    public abstract class SailingShip : MonoBehaviour
    {
        [Header("General")]
        [SerializeField] private Transform playerTransform;
        [Space]
        [SerializeField] private Transform sailTarget;

        [field: Header("Ship Config")]
        [field: SerializeField, Min(1)] protected float Speed { get; private set; }
        [field: SerializeField] protected float PermissibleStopRange { get; private set; } = 0.5f;

        [field:Header("Player Detection")]
        [field: SerializeField] protected PlayerDetector PlayerDetector { get; private set; }
        [SerializeField, Min(1f)] private float detectionRadius = 3f;

        [field:Header("Ship - Flag Animator")]
        [field: SerializeField] protected Animator FlagAnimator { get; private set; }
        [Space]
        [SerializeField] private AnimationClip idleAnimationClip;
        [SerializeField] private AnimationClip windAnimationClip;

        [Header("Ship - SFX")]
        [SerializeField] private AudioClip shipAudioClip;

        public event Action OnDestinationReached;
        private AudioSystem audioSystem;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        public void Initialize()
        {
            PlayerDetector.Initialize(detectionRadius);
        }

        protected void MarkAsReachedDestination() => OnDestinationReached?.Invoke();

        protected IEnumerator SailTowardsTarget()
        {
            audioSystem.PlaySFX(shipAudioClip, true);

            while (IsNearTarget == false)
            {
                playerTransform.Translate(Speed * Time.deltaTime * Vector3.right);
                transform.Translate(Speed * Time.deltaTime * Vector3.right);
                yield return null;
            }

            audioSystem.StopSFX();
        }

        public void SetFlagToIdle() => FlagAnimator.Play(idleAnimationClip.name);
        public void SetFlagToWind() => FlagAnimator.Play(windAnimationClip.name);
        private bool IsNearTarget => sailTarget.position.x - PermissibleStopRange < transform.position.x && transform.position.x < sailTarget.position.x + PermissibleStopRange;
    }
}