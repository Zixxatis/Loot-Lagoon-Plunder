using System;
using System.Collections;
using UnityEngine;

namespace CGames
{
    public class HealthSystem : MonoBehaviour, IDamageable, IKnockable
    {
        private const int CriticalMultiplier = 2;
        private const float KnockBackResistMultiplier = 0.5f;

        public event Action OnHealthChanged;
        public event Action OnDamageTaken;
        public event Action OnDeath;

        public int Health { get; private set; }
        public bool IsRecovering { get; private set; }
        public bool IsInvincible { get; private set; }
        public bool HasHealthPoints => Health > 0;
        public bool HasFullHP => Health == healthSystemConfig.MaxHealth;
        public bool CanBeDamaged => HasHealthPoints && IsRecovering == false && IsInvincible == false;

        private Rigidbody2D body;
        private HealthSystemConfig healthSystemConfig;
        private CharacterData characterData;
        
        public void Initialize(Rigidbody2D body, HealthSystemConfig healthSystemConfig, CharacterData characterData)
        {
            this.body = body;
            this.healthSystemConfig = healthSystemConfig;
            this.characterData = characterData;

            Health = healthSystemConfig.MaxHealth;
            OnHealthChanged += ClampHealth;
        }

        public void TakeDamage(int damageAmount, bool isCritical = false)
        {
            if(CanBeDamaged == false || damageAmount <= 0)
                return;

            if(isCritical)
                damageAmount *= CriticalMultiplier;

            Health -= damageAmount;
            OnHealthChanged?.Invoke();

            DamageInfoCanvas.DisplayDamage(characterData.IsEnemy, damageAmount, transform.position, isCritical);
            
            if (HasHealthPoints)
            {
                StartCoroutine(BeInRecoveryState());
                OnDamageTaken?.Invoke();
            }
            else
            {
                healthSystemConfig.DisableVampiricAffect();
                OnDeath?.Invoke();
            }
        }      

        private IEnumerator BeInRecoveryState()
        {
            float timer = 0f;
            IsRecovering = true;

            while(timer < healthSystemConfig.RecoveryWindowInSeconds)
            {
                if(characterData.IsEnemy)
                    characterData.ResetInput();
                    
                timer += Time.deltaTime;   
                yield return null;
            }

            IsRecovering = false;
        }
        
        public void KnockBack(Vector2 knockBackForce, bool shouldKnockBackToLeft)
        {
            if(HasHealthPoints == false)
                return;

            if(healthSystemConfig.IsKnockable == false || body.bodyType != RigidbodyType2D.Dynamic)
                return;

            Vector2 knockBackDirection = shouldKnockBackToLeft? new Vector2(knockBackForce.x * -1, knockBackForce.y) : knockBackForce;

            if(healthSystemConfig.HasResistToKnockBack)
                knockBackDirection *= KnockBackResistMultiplier;

            body.AddForce(knockBackDirection, ForceMode2D.Impulse);
        } 

        public void HealInPercents(int percent)
        {
            float healAmount = healthSystemConfig.MaxHealth * percent * 0.01f;

            Heal((int)healAmount);
        }

        public void Heal(int healAmount)
        {
            if(Health == healthSystemConfig.MaxHealth)
                return;

            DamageInfoCanvas.DisplayHeal(healAmount, transform.position);
            
            Health += healAmount;
            OnHealthChanged?.Invoke();
        }

        private void ClampHealth() => Health = Mathf.Clamp(Health, 0, healthSystemConfig.MaxHealth);
        public void MakeInvincible(bool shouldBeInvincible) => IsInvincible = shouldBeInvincible;

        /// <summary> Sets health to the given value, without invoking any events. </summary>
        public void SetHealth(int healthAmount) => Health = healthAmount;

        /// <summary> Sets health to the maximum, without invoking any events. </summary>
        public void SetToFullHP() => Health = healthSystemConfig.MaxHealth;

        private void OnDestroy() => OnHealthChanged -= ClampHealth;
    }
}