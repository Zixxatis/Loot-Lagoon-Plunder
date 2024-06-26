using System;
using CGames.Utilities;
using UnityEngine;

namespace CGames
{   
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(SpriteRenderer))]
    public abstract class DestroyableObstacle : MonoBehaviour, IDamageable
    {
        [SerializeField] private AnimationClip destroyedAnimationClip;

        protected event Action OnObjectHit;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Collider2D objectCollider;

        private void Awake()
        {
            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            objectCollider = GetComponent<Collider2D>();
        }

        public void TakeDamage(int amount, bool isCritical = false)
        {
            if(amount <= 0)
                return;
            
            ShowDestructionAnimation();
            MoveToDestroyedLayers();

            OnObjectHit?.Invoke();
        }

        private void ShowDestructionAnimation()
        {
            animator.Play(destroyedAnimationClip.name);
        }

        protected void MoveToDestroyedLayers()
        {
            spriteRenderer.sortingLayerName = LayerUtilities.DestroyedSortingLayerName;

            objectCollider.gameObject.layer = LayerUtilities.DestroyedLayerIndex;
            objectCollider.isTrigger = true;
        }
    }
}