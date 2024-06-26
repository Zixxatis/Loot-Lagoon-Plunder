using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class SharkNPC : EnemyNPC
    {
        [Header("Shark NPC Components")]
        [SerializeField] private MovementModule movementModule;
        [SerializeField] private JumpingModule jumpingModule;
        [Space]
        [SerializeField] private CombatModule combatModule;

        [Header("Shark State Clips")]
        [SerializeField] private StateClip idlingStateClip;
        [SerializeField] private StateClip runningStateClip;
        [SerializeField] private StateClip attackingStateClip;
        [Space]
        [SerializeField] private StateClip jumpingStateClip;
        [SerializeField] private StateClip fallingStateClip;
        [Space]
        [SerializeField] private StateClip hitStateClip;
        [SerializeField] private StateClip deathStateClip;

        protected override float DeathAnimationDuration => deathStateClip.AnimationDuration;

        protected override List<IState> GetCharacterStates()
        {
            return new()
            {
                new IdlingState(StateMachine, this, idlingStateClip),
                new RunningState(StateMachine, this, runningStateClip),
                new AttackingState(StateMachine, this, attackingStateClip),

                new JumpingState(StateMachine, this, jumpingStateClip),
                new FallingState(StateMachine, this, fallingStateClip),

                new HitState(StateMachine, this, hitStateClip),
                new DeathState(StateMachine, this, deathStateClip),
            };
        }
        
        protected override Queue<ITask> GetDefaultTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new ChargeRunTask(this));
            taskQueue.Enqueue(new JumpTask(this, Jump));
            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.GeneralAwait)));

            return taskQueue;
        }

        protected override Queue<ITask> GetLongRangeTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new ParkourToAttackTask
            (
                this,
                EnterAttackingState,
                Jump,
                () => combatModule.AttackPointPosition,
                () => EnemyBrain.LongRangeDetector.DetectedCharacter.transform.position)
            );

            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.AfterAttackRecovery)));

            return taskQueue;
        }

        protected override void InitializeEnemy()
        {
            movementModule.Initialize(this);
            jumpingModule.Initialize(this);

            combatModule.Initialize(CharacterAttributes.GetAttackConfig(AttackType.Default), CharacterData, () => CharacterViewDirectionSign, (x) => HealthSystem.Heal(x));

            HealthSystem.OnDeath += combatModule.StopAttack;
        }

        private void Jump()
        {
            StateMachine.SwitchState<JumpingState>();
            jumpingModule.Jump();
        }

        protected override void EnterAttackingState()
        {
            combatModule.Attack();
            StateMachine.SwitchState<AttackingState>();
        }
        
        protected override void OnDeath()
        {
            base.OnDeath();

            combatModule.enabled = false;
            HealthSystem.OnDeath -= combatModule.StopAttack;
        }
    }
}