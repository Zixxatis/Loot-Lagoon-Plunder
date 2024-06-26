using System.Collections;
using UnityEngine;

namespace CGames
{
    public class StartingShip : SailingShip
    {
        [Header("Starting Ship - Extra")]
        [SerializeField] private AnimationClip toIdleAnimationClip;
        [Space]
        [SerializeField] private Transform escapeTarget;

        public IEnumerator SailToTerrain()
        {
            yield return StartCoroutine(SailTowardsTarget());

            MarkAsReachedDestination();

            StartCoroutine(ChangeFlagToIdle());
            StartCoroutine(WaitForPlayerToLeave());
        }

        private IEnumerator WaitForPlayerToLeave()
        {
            while(PlayerDetector.IsTargetInRange)
            {
                yield return null;
            }
            
            StartCoroutine(SailTowardsEscapePoint());
        }

        private IEnumerator SailTowardsEscapePoint()
        {
            while (IsNearEscapeTarget == false)
            {
                transform.Translate(Speed * 2 * Time.deltaTime * Vector3.left);
                yield return null;
            }
        }

        private IEnumerator ChangeFlagToIdle()
        {
            FlagAnimator.Play(toIdleAnimationClip.name);

            yield return new WaitForSeconds(toIdleAnimationClip.length);

            SetFlagToIdle();
        }
        
        private bool IsNearEscapeTarget => escapeTarget.position.x - PermissibleStopRange < transform.position.x && transform.position.x < escapeTarget.position.x + PermissibleStopRange;
    }
}