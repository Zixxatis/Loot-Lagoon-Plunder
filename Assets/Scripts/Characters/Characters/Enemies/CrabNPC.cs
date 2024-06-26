using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class CrabNPC : EnemyNPC
    {
        [Header("Crab NPC Components")]
        [SerializeField] private MovementModule movementModule;
        [SerializeField] private CombatModule combatModule;
        
        [Header("Crab - Wandering Target")]
        [SerializeField] private Transform leftWanderTarget;
        [SerializeField] private Transform rightWanderTarget;

        [Header("Crab State Clips")]
        [SerializeField] private StateClip idlingStateClip;
        [SerializeField] private StateClip runningStateClip;
        [SerializeField] private StateClip attackingStateClip;
        [Space]
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

                new FallingState(StateMachine, this, fallingStateClip),

                new HitState(StateMachine, this, hitStateClip),
                new DeathState(StateMachine, this, deathStateClip),
            };
        }

        protected override Queue<ITask> GetDefaultTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new WanderTask(this, leftWanderTarget, rightWanderTarget));
            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.GeneralAwait)));

            return taskQueue;
        }

        protected override Queue<ITask> GetLongRangeTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new RunAwayTask(this, () => EnemyBrain.LongRangeDetector.DetectedCharacter.transform.position));
            taskQueue.Enqueue(new ApproachToAttackTask
            (
                this,
                EnterAttackingState,
                () => combatModule.AttackPointPosition,
                () => EnemyBrain.LongRangeDetector.DetectedCharacter.transform.position)
            );

            return taskQueue;
        }

        protected override Queue<ITask> GetCloseRangeTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new ApproachToAttackTask
            (
                this,
                EnterAttackingState,
                () => combatModule.AttackPointPosition,
                () => EnemyBrain.CloseRangeDetector.DetectedCharacter.transform.position)
            );

            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.AfterAttackRecovery)));

            return taskQueue;
        }

        protected override void InitializeEnemy()
        {
            movementModule.Initialize(this);

            combatModule.Initialize(CharacterAttributes.GetAttackConfig(AttackType.Default), CharacterData, () => CharacterViewDirectionSign, (x) => HealthSystem.Heal(x));

            HealthSystem.OnDeath += combatModule.StopAttack;
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