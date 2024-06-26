using System;
using System.Collections;
using CGames.Utilities;
using UnityEngine;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Collider2D), typeof(Animator))]
    public abstract class Collectable : MonoBehaviour
    {
        [Header("Collectable Elements")]
        [SerializeField] private Animator animator;
        
        [Header("Pick-up Effects")]
        [SerializeField] private AudioClip pickUpSFX;
        [Space]
        [SerializeField] private AnimationClip postPickupAnimationClip;

        private bool isPickedUp;
        
        private Action<AudioClip> playOneShotEffectAction;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.playOneShotEffectAction = audioSystem.PlayOneShotSFX;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (isPickedUp || other.CompareTag(LayerUtilities.PlayerTag) == false)
                return;
            else 
                isPickedUp = true;

            UpdatePlayerCollectables();
            UpdateHUD();

            playOneShotEffectAction?.Invoke(pickUpSFX);
            StartCoroutine(PlayPickUpAnimation());
        }

        private IEnumerator PlayPickUpAnimation()
        {
            animator.StopPlayback();
            animator.Play(postPickupAnimationClip.name);

            yield return new WaitForEndOfFrame();

            while(animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
            {
                yield return null;
            }

            Destroy(gameObject);
        }

        public abstract void UpdatePlayerCollectables();
        public abstract void UpdateHUD();
    }
}
