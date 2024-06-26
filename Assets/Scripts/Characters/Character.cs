using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Rigidbody2D))]
    public abstract class Character : MonoBehaviour
    {
        [field: Header("Character Components")]
        [field: SerializeField] public GroundSensor GroundSensor { get; private set; }
        [field: SerializeField] public HealthSystem HealthSystem { get; private set; }
        [field: SerializeField] public CharacterAnimator CharacterAnimator { get; private set; }

        [field: Header("Character Configuration")]
        [SerializeField] private CharacterConfig characterConfig;
        
        public CharacterAttributes CharacterAttributes { get; private set; }

        public Rigidbody2D Body { get; private set; }
        public CharacterData CharacterData { get; private set; }
        protected StateMachine StateMachine { get; private set; }

        /// <summary> Returns 1 if facing right, and -1 if facing left. </summary>
        public float CharacterViewDirectionSign => Mathf.Sign(Body.transform.localScale.x);

        private void Awake()
        {
            Body = GetComponent<Rigidbody2D>();

            PrepareCharacterData();

            PrepareStateMachine();
            PrepareGroundSensor();
            PrepareHealthSystem();

            InitializeCharacter();
        }
        
        private void PrepareCharacterData()
        {
            CharacterAttributes = GetCharacterAttributes(characterConfig);
            CharacterData = new(this is not PlayerCharacter);
        }

        protected abstract CharacterAttributes GetCharacterAttributes(CharacterConfig characterConfig);

        private void PrepareStateMachine()
        {
            CharacterAnimator.Initialize();

            StateMachine = new();
            StateMachine.Initialize(new(GetCharacterStates()));
        }

        protected abstract List<IState> GetCharacterStates();

        private void PrepareGroundSensor()
        {
            GroundSensor.Initialize();
        }

        protected void PrepareHealthSystem()
        {
            HealthSystem.Initialize(Body, CharacterAttributes.HealthSystemConfig, CharacterData);
            
            HealthSystem.OnDamageTaken += StateMachine.SwitchState<HitState>;
            HealthSystem.OnDeath += OnDeath;
        }

        protected abstract void InitializeCharacter();

        protected virtual void Update() => StateMachine.Update();

        protected virtual void OnDeath()
        {
            StateMachine.SwitchState<DeathState>();
        }

        protected virtual void OnDestroy()
        {
            HealthSystem.OnDamageTaken -= StateMachine.SwitchState<HitState>;
            HealthSystem.OnDeath -= OnDeath;
        }

    #region Context Menu Methods

        [ContextMenu("Kill Unit")] private void Editor_Kill() => HealthSystem.TakeDamage(HealthSystem.Health, false);
        [ContextMenu("Set to 1 HP")] private void Editor_SetToOneHP() => HealthSystem.TakeDamage(HealthSystem.Health - 1, false);
        [ContextMenu("Heal Unit")] private void Editor_Heal() => HealthSystem.SetToFullHP();

    #endregion
    }
}