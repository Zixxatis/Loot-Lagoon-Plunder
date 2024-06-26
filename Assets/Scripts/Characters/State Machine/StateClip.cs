using System;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [Serializable]
    public class StateClip
    {
        [SerializeField] private AnimationClip animationClip;
        [SerializeField] private List<AnimationClip> animationClipsList;
        [Space]
        [SerializeField] private bool hasMultipleAnimations;
        [Space]
        [SerializeField] private AudioClip soundEffectClip;
        [SerializeField] private bool shouldPlayAsOneShot = true;
        [SerializeField] private bool shouldPlayOnlyWhenVisible = true;
        [SerializeField] private bool shouldSetOnLoop;

        public bool IsInitialized { get ; private set; }
        private AudioSystem audioSystem;

        public float AnimationDuration => animationClip.length;

        public void Initialize(AudioSystem audioSystem)
        {
            this.audioSystem = audioSystem;
            IsInitialized = true;
        }

        public AnimationClip GetAnimationClip()
        {   
            if(hasMultipleAnimations)
                return animationClipsList.GetRandomItem();
            else
                return animationClip;
        }

        public void PlaySoundEffect(bool isVisible)
        {
            if(soundEffectClip == null)
                return;

            if(shouldPlayAsOneShot)
            {
                if(shouldPlayOnlyWhenVisible && isVisible == false)
                    return;

                audioSystem.PlayOneShotSFX(soundEffectClip);
            }
            else
                audioSystem.PlaySFX(soundEffectClip, shouldSetOnLoop);
        }

        public void StopSoundEffect()
        {
            if(shouldPlayAsOneShot)
                return;
            
            audioSystem.StopSFX();
        }
    }
}