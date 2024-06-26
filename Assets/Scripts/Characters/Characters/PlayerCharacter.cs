using System.Collections.Generic;
using UnityEngine;
using CGames.Utilities;
using Zenject;

namespace CGames
{
    public class PlayerCharacter : Character
    {
        [field: Header("Player Components")]
        [field: SerializeField] public PlayerInputReceiver PlayerInputReceiver { get; private set; }
        [Space]
        [SerializeField] private Collider2D torsoCollider;
        [field: Space]
        [field: SerializeField] public MovementModule MovementModule { get; private set; }
        [field: SerializeField] public JumpingModule JumpingModule { get; private set; }
        [field: Space]
        [field: SerializeField] public CombatModule GroundCombatModule { get; private set; }
        [field: SerializeField] public CombatModule AirCombatModule { get; private set; }

        [Header("Player State Clips")]
        [SerializeField] private StateClip idlingStateClip;
        [SerializeField] private StateClip runningStateClip;
        [SerializeField] private StateClip attackingStateClip;
        [Space]
        [SerializeField] private StateClip jumpingStateClip;
        [SerializeField] private StateClip fallingStateClip;
        [SerializeField] private StateClip airAttackingStateClip;
        [Space]
        [SerializeField] private StateClip hitStateClip;
        [SerializeField] private StateClip deathStateClip;

        public PlayerAttributes PlayerAttributes => (PlayerAttributes)CharacterAttributes;

        private PlayerDataView playerDV;
        private SceneLoader sceneLoader;
        private HeadsUpDisplay headsUpDisplay;
        private ILosableLevelController losableLevelController;

        [Inject]
        private void Construct(PlayerDataView playerDV, SceneLoader sceneLoader, HeadsUpDisplay headsUpDisplay, ILosableLevelController losableLevelController)
        {
            this.playerDV = playerDV;
            this.sceneLoader = sceneLoader;
            this.headsUpDisplay = headsUpDisplay;
            this.losableLevelController = losableLevelController;
        }

        protected override CharacterAttributes GetCharacterAttributes(CharacterConfig characterConfig)
        {
            return new PlayerAttributes
            (
                characterConfig, 
                playerDV.UpgradesDictionary, 
                ScheduleRegenerationAtTheEndOfTheLevel
            );
        }

        protected override List<IState> GetCharacterStates()
        {
            return new()
            {
                new IdlingState(StateMachine, this, idlingStateClip),
                new RunningState(StateMachine, this, runningStateClip),
                new AttackingState(StateMachine, this, attackingStateClip),

                new JumpingState(StateMachine, this, jumpingStateClip),
                new FallingState(StateMachine, this, fallingStateClip),
                new AirAttackingState(StateMachine, this, airAttackingStateClip),

                new HitState(StateMachine, this, hitStateClip),
                new DeathState(StateMachine, this, deathStateClip),
            };
        }
        
        protected override void InitializeCharacter()
        {
            UpdateHealthSystem();

            PreparePlayerInput();
            PrepareMovementModules();
            PrepareCombatModules();
        }

        private void UpdateHealthSystem()
        {
            if (playerDV.HealthAtTheEndOfLevel > 0)
                HealthSystem.SetHealth(playerDV.HealthAtTheEndOfLevel);

            headsUpDisplay.SetupHealthInfo(() => CharacterAttributes.HealthSystemConfig.MaxHealth, () => HealthSystem.Health);

            HealthSystem.OnHealthChanged += headsUpDisplay.UpdateHealthInfo;
            HealthSystem.OnDamageTaken += DamageInfoCanvas.ShowOverlayEffect;
            HealthSystem.OnDeath += AirCombatModule.StopAttack;
            HealthSystem.OnDeath += GroundCombatModule.StopAttack;

            sceneLoader.OnLoadingStarted += playerDV.SaveHealthAmountAtTheEndOfLevel;
        }

        private void PreparePlayerInput()
        {
            PlayerInputReceiver.Initialize(CharacterData);

            PlayerInputHandler.OnJumpButtonPressed += Jump;
            PlayerInputHandler.OnAttackButtonPressed += EnterAttackingState;

            CharacterData.OnAttackEnter += PlayerInputHandler.DisableAttack;
            CharacterData.OnAttackExit += PlayerInputHandler.EnableAttack;
            CharacterData.OnAirBorneEnter += PlayerInputHandler.DisableJump;
            CharacterData.OnAirBorneExit += PlayerInputHandler.EnableJump;
        }
        
        private void Jump()
        {
            StateMachine.SwitchState<JumpingState>();
            JumpingModule.Jump();
        }

        private void PrepareMovementModules()
        {
            MovementModule.Initialize(this);
            JumpingModule.Initialize(this);
        }

        private void PrepareCombatModules()
        {
            GroundCombatModule.Initialize(CharacterAttributes.GetAttackConfig(AttackType.Default), CharacterData, () => CharacterViewDirectionSign, (x) => HealthSystem.Heal(x));
            AirCombatModule.Initialize(CharacterAttributes.GetAttackConfig(AttackType.Airborne), CharacterData, () => CharacterViewDirectionSign, (x) => HealthSystem.Heal(x));
        }

        private void EnterAttackingState()
        {
            if(GroundSensor.IsGrounded)
            {
                StateMachine.SwitchState<AttackingState>();
                GroundCombatModule.Attack();
            }
            else
            {
                StateMachine.SwitchState<AirAttackingState>();
                AirCombatModule.Attack();
            }
        }

        private void ScheduleRegenerationAtTheEndOfTheLevel(int regenerationAmount)
        {
            if(losableLevelController is LevelController levelController)
            {
                levelController.OnEscapeShipEnterEvent.AddListener(() => HealthSystem.Heal(regenerationAmount));
            }
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            GroundSensor.gameObject.layer = LayerUtilities.DestroyedLayerIndex;
            torsoCollider.gameObject.layer = LayerUtilities.DestroyedLayerIndex;

            losableLevelController.SetAsLost();
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            PlayerInputHandler.OnJumpButtonPressed -= Jump;
            PlayerInputHandler.OnAttackButtonPressed -= EnterAttackingState;

            CharacterData.OnAttackEnter -= PlayerInputHandler.DisableAttack;
            CharacterData.OnAttackExit -= PlayerInputHandler.EnableAttack;
            CharacterData.OnAirBorneEnter -= PlayerInputHandler.DisableJump;
            CharacterData.OnAirBorneExit -= PlayerInputHandler.EnableJump;

            HealthSystem.OnHealthChanged -= headsUpDisplay.UpdateHealthInfo;
            HealthSystem.OnDamageTaken -= DamageInfoCanvas.ShowOverlayEffect;
            HealthSystem.OnDeath -= AirCombatModule.StopAttack;
            HealthSystem.OnDeath -= GroundCombatModule.StopAttack;

            sceneLoader.OnLoadingStarted -= playerDV.SaveHealthAmountAtTheEndOfLevel;
        }

    #region Context Menu Methods
        
        [ContextMenu("Get 20 coins")] 
        private void Editor_GetCoins()
        {
            playerDV.CollectCoin(20);
            headsUpDisplay.UpdateCoinsInfo();
        }

    #endregion
    }
}