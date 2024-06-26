using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class LoadingScreenCanvas : MonoBehaviour
    {   
        [Header("Canvas Elements")]
        [SerializeField] private GameObject blocker;
        [Space]
        [SerializeField] private Animator animator;

        [Header("Default Clips")]
        [SerializeField] private AnimationClip preTransitionClip;
        [SerializeField] private AudioClip preTransitionAudio;
        [Space]
        [SerializeField] private AnimationClip postTransitionClip;
        [SerializeField] private AudioClip postTransitionAudio;

        public bool IsAnimationInProgress { get; private set; }
        
        private Action<AudioClip> playOneShotEffectAction;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.playOneShotEffectAction = audioSystem.PlayOneShotSFX;
        }

        private void Awake()
        {
            animator.updateMode = AnimatorUpdateMode.UnscaledTime;
            DisableElements();

            preTransitionAudio.LoadAudioData();
        }

        public void PlayPreTransitionClip()
        {
            playOneShotEffectAction?.Invoke(preTransitionAudio);
            StartCoroutine(PlayTransition(preTransitionClip));
        }

        public IEnumerator PlayPostTransitionClip()
        {
            playOneShotEffectAction?.Invoke(postTransitionAudio);
            yield return StartCoroutine(PlayTransition(postTransitionClip));
            DisableElements();
        }

        private IEnumerator PlayTransition(AnimationClip transitionClip)
        {
            IsAnimationInProgress = true;
            EnableElements();

            animator.Play(transitionClip.name);
            yield return new WaitForSecondsRealtime(transitionClip.length);
            
            IsAnimationInProgress = false;
        }

        private void EnableElements()
        {
            animator.ActivateGameObject();
            blocker.ActivateObject();
        }

        private void DisableElements()
        {
            animator.DeactivateGameObject();
            blocker.DeactivateObject();
        }
    }
}