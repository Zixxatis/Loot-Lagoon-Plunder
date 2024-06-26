using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class JumperObject : MonoBehaviour
    {
        [SerializeField] private JumperDetection jumperDetection;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [Space]
        [SerializeField] private Sprite defaultSprite;
        [SerializeField] private Sprite activeSprite;
        [SerializeField, Min(0.1f)] private float activeStateDuration = 0.15f;
        [Space]
        [SerializeField] private AudioClip activationSFX;

        private Action<AudioClip> playOneShotEffectAction;

        [Inject]
        private void Construct(AudioSystem audioSystem)
        {
            this.playOneShotEffectAction = audioSystem.PlayOneShotSFX;
        }

        private void Awake()
        {
            jumperDetection.OnJump += ActivateSpring;
        }

        private void ActivateSpring()
        {
            playOneShotEffectAction?.Invoke(activationSFX);
            StartCoroutine(ChangeSprite());
        }

        private IEnumerator ChangeSprite()
        {
            spriteRenderer.sprite = activeSprite;

            yield return new WaitForSeconds(activeStateDuration);

            spriteRenderer.sprite = defaultSprite;
        }

        private void OnDestroy()
        {
            StopAllCoroutines();
            jumperDetection.OnJump -= ActivateSpring;
        }
    }
}