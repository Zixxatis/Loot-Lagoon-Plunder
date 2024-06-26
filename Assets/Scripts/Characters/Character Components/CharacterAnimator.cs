using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private AudioSystem audioSystem;
        private Animator animator;
        private SpriteRenderer spriteRenderer;

        private AnimationClip currentAnimationClip;
        private Action OnLoopReachedAction;

        private bool IsAnimationPlaying => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
        }

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
        }

        public void SetAnimationOnLoop(StateClip stateClip)
        {
            ResetAnimatorSpeed();

            currentAnimationClip = stateClip.GetAnimationClip();

            animator.Play(currentAnimationClip.name);

            if(stateClip.IsInitialized == false)
                stateClip.Initialize(audioSystem);

            stateClip.PlaySoundEffect(spriteRenderer.isVisible);
        }

        public void SetSingleAnimation(StateClip stateClip, float duration, Action action) 
        {
            OnLoopReachedAction = action;

            SetAnimationOnLoop(stateClip);

            ChangeAnimatorSpeed(duration);
            StartCoroutine(WaitForAnimationToEnd());
        }

        private IEnumerator WaitForAnimationToEnd()
        {
            yield return null;

            while(IsAnimationPlaying)
            {
                yield return null;
            }

            OnLoopReachedAction?.Invoke();

            OnLoopReachedAction = null;
        }

        private void ChangeAnimatorSpeed(float requiredDurationInSeconds) => animator.speed = currentAnimationClip.length / requiredDurationInSeconds;
        private void ResetAnimatorSpeed() => animator.speed = 1;
    }
}