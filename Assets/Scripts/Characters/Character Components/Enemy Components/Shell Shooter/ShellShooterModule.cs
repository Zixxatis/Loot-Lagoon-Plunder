using System.Collections;
using UnityEngine;
using UnityEngine.Pool;

namespace CGames
{
    public class ShellShooterModule : MonoBehaviour
    {
        [SerializeField] private ShellProjectile projectilePrefab;
        [Space]
        [SerializeField] private Transform projectileSpawnPoint;
        [SerializeField] private float delayBeforeInstantiating = 0.3f;

        private ObjectPool<ShellProjectile> projectilePool;
        private CharacterData characterData;

        public void Initialize(CharacterData characterData)
        {
            this.characterData = characterData;

            projectilePool = new
            (
                CreateNewProjectile,
                x => x.SpawnProjectile(),
                x => x.DeactivateGameObject(),
                x => Destroy(x.gameObject)
            );
        }

        public void Shoot()
        {
            if(characterData.IsAttacking)
                return;

            characterData.EnterAttack();
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {
            yield return new WaitForSeconds(delayBeforeInstantiating);

            projectilePool.Get();

            characterData.ExitAttack();
        }

        private ShellProjectile CreateNewProjectile()
        {
            ShellProjectile projectile = Instantiate(projectilePrefab, projectileSpawnPoint);
            projectile.Initialize(projectileSpawnPoint, () => projectilePool.Release(projectile));

            return projectile;
        }
    }
}