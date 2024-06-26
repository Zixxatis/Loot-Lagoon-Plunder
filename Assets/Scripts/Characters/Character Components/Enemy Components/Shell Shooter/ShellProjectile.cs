using System;
using System.Collections;
using UnityEngine;
using CGames.Utilities;

namespace CGames
{
    [RequireComponent(typeof(Animator), typeof(Collider2D), typeof(SpriteRenderer))]
    public class ShellProjectile : MonoBehaviour
    {
        [SerializeField] private ProjectileConfig projectileConfig;
        [Space]
        [SerializeField] private AnimationClip idleAnimationClip;
        [SerializeField] private AnimationClip destroyedAnimationClip;

        private bool hasTouchedSomething;

        private Animator animator;
        private SpriteRenderer spriteRenderer;
        private Collider2D objectCollider;

        private Transform projectileSpawnPoint;
        private Action returnToPoolAction;

        public void Initialize(Transform projectileSpawnPoint, Action returnToPoolAction)
        {
            this.projectileSpawnPoint = projectileSpawnPoint;
            this.returnToPoolAction = returnToPoolAction;

            animator = GetComponent<Animator>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            objectCollider = GetComponent<Collider2D>();
        }

        public void SpawnProjectile()
        {
            this.transform.position = projectileSpawnPoint.position;
            MoveToProjectileLayers();

            this.ActivateGameObject();
            animator.Play(idleAnimationClip.name);

            StartCoroutine(Fly());
        }

        private void MoveToProjectileLayers()
        {
            spriteRenderer.sortingLayerName = LayerUtilities.DefaultSortingLayerName;

            objectCollider.gameObject.layer = LayerUtilities.ProjectilesLayerIndex;
            objectCollider.isTrigger = false;
        }

        private IEnumerator Fly()
        {
            hasTouchedSomething = false;

            while(hasTouchedSomething == false)
            {
                transform.Translate(projectileConfig.ProjectileSpeed * Time.deltaTime * Vector3.right);
                yield return null;
            }

            StartCoroutine(DestroyProjectile());
        }

        private IEnumerator DestroyProjectile()
        {
            animator.Play(destroyedAnimationClip.name);
            MoveToDestroyedLayers();

            yield return new WaitForSeconds(projectileConfig.PreDestructionAwaitDuration);

            returnToPoolAction?.Invoke();
        }

        private void MoveToDestroyedLayers()
        {
            spriteRenderer.sortingLayerName = LayerUtilities.DestroyedSortingLayerName;

            objectCollider.gameObject.layer = LayerUtilities.DestroyedLayerIndex;
            objectCollider.isTrigger = true;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag(LayerUtilities.PlayerTag))
            {
                IDamageable damagedObject = collision.transform.GetComponentInChildren<IDamageable>();

                damagedObject.TakeDamage(projectileConfig.Damage);

                if(damagedObject is IKnockable knockableObject)
                {
                    bool isPlayerAtLeftSide = collision.transform.position.x < this.GetComponentInParent<Character>().transform.position.x;
                    knockableObject.KnockBack(projectileConfig.KnockBackForce, isPlayerAtLeftSide);
                }
            }

            hasTouchedSomething = true;
        }
    }
}
