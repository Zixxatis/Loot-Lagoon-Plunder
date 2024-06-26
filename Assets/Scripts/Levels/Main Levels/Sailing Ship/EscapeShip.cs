using System;
using System.Collections;
using UnityEngine;

namespace CGames
{
    public class EscapeShip : SailingShip
    {
        [Header("Escape Ship - Extra")]
        [SerializeField] private AnimationClip toWindAnimationClip;

        public event Action OnPlayerEnteredShip;

        public IEnumerator WaitForPlayerToAppear()
        {
            while(PlayerDetector.IsTargetInRange == false)
            {
                yield return null;
            }

            OnPlayerEnteredShip?.Invoke();
            
            StartCoroutine(SailFromTerrain());
        }

        public IEnumerator SailFromTerrain()
        {
            yield return StartCoroutine(ChangeFlagToWind());
            yield return StartCoroutine(SailTowardsTarget());

            MarkAsReachedDestination();
        }

        private IEnumerator ChangeFlagToWind()
        {
            FlagAnimator.Play(toWindAnimationClip.name);

            yield return new WaitForSeconds(toWindAnimationClip.length);

            SetFlagToWind();
        }
    }
}