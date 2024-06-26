using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class CombatModule : MonoBehaviour
    {
        [SerializeField] private LayerMask damageableLayers;
        [Space]
        [SerializeField] private Transform attackPoint;

        [Header("Editor")]
        [SerializeField] private float previewRadius;

        public Vector3 AttackPointPosition => attackPoint.position;

        private AttackConfig attackConfig;
        private CharacterData characterData;
        private Func<float> getCharacterDirectionSign;
        private Action<int> healCharacterAction;

        private void OnDrawGizmos()
        {
            Gizmos.DrawWireSphere(attackPoint.position, previewRadius);
        }

        public void Initialize(AttackConfig attackConfig, CharacterData characterData, Func<float> getCharacterDirectionSign, Action<int> healCharacterAction)
        {
            this.attackConfig = attackConfig;
            this.characterData = characterData;
            this.getCharacterDirectionSign = getCharacterDirectionSign;
            this.healCharacterAction = healCharacterAction;
        }

        public void Attack()
        {
            if(characterData.IsAttacking)
                return;

            characterData.EnterAttack();
            StartCoroutine(AttackCoroutine());
        }

        private IEnumerator AttackCoroutine()
        {   
            bool isAttackCritical = IsThisAttackCritical();
            HashSet<Collider2D> hitEnemiesSet = new();
            float timer = 0f;

            yield return new WaitForSeconds(attackConfig.DelayBeforeDamagingInSeconds);

            while(timer < attackConfig.DurationInSeconds)
            {
                DetectEnemiesInAttackRange(hitEnemiesSet, isAttackCritical);

                timer += Time.deltaTime;
                yield return null;
            }

            characterData.ExitAttack();
        }

        private void DetectEnemiesInAttackRange(HashSet<Collider2D> hitEnemiesSet, bool isAttackCritical)
        {
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(AttackPointPosition, attackConfig.AttackRangeRadius, damageableLayers);

            foreach (Collider2D enemyCollider in hitEnemies)
            {
                if (hitEnemiesSet.Contains(enemyCollider))
                    continue;

                Character hitCharacter = enemyCollider.GetComponentInParent<Character>();

                if (enemyCollider.gameObject.TryGetComponent(out IDamageable damagedObject) == false)
                    damagedObject = hitCharacter.GetComponentInChildren<IDamageable>();

                damagedObject.TakeDamage(attackConfig.Damage, isAttackCritical);

                if (damagedObject is IKnockable knockableObject)
                {
                    bool shouldKnockBackToLeft = getCharacterDirectionSign() == -1;
                    knockableObject.KnockBack(attackConfig.KnockBackForce, shouldKnockBackToLeft);
                }

                if(hitCharacter != null && hitCharacter.CharacterAttributes.HealthSystemConfig.IsCountedForVampiric && attackConfig.VampiricHealAmount > 0)
                {
                    int healAmount = isAttackCritical? attackConfig.VampiricHealAmount * 2 : attackConfig.VampiricHealAmount;
                    healCharacterAction?.Invoke(healAmount);
                }
                
                hitEnemiesSet.Add(enemyCollider);
            }
        }
        
        private bool IsThisAttackCritical()
        {
            if(attackConfig.CriticalChanceInPercent > 0)
            {
                int percent = UnityEngine.Random.Range(1, 101);
                return percent <= attackConfig.CriticalChanceInPercent;
            }
            else
                return false;
        }

        public void StopAttack()
        {
            if(characterData.IsAttacking)
            {
                StopAllCoroutines();
                characterData.ExitAttack();
            }
        }
    }
}