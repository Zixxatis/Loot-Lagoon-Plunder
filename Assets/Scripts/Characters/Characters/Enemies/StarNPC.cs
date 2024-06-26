using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class StarNPC : EnemyNPC
    {
        [Header("Star NPC Components")]
        [SerializeField] private MovementModule movementModule;
        [Space]
        [SerializeField] private CombatModule combatModule;

        [Header("Star State Clips")]
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

            taskQueue.Enqueue(new PatrolTask(this));
            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.PreStrikeAwait)));
            taskQueue.Enqueue(new AttackTask(this, EnterAttackingState));
            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.AfterAttackRecovery)));

            return taskQueue;
        }

        protected override void InitializeEnemy()
        {
            movementModule.Initialize(this);

            combatModule.Initialize(CharacterAttributes.GetAttackConfig(AttackType.Default), CharacterData, () => CharacterViewDirectionSign, (x) => HealthSystem.Heal(x));

            CharacterData.OnAttackEnter += () => HealthSystem.MakeInvincible(true);
            CharacterData.OnAttackExit += () => HealthSystem.MakeInvincible(false);
        }

        protected override void EnterAttackingState()
        {
            combatModule.Attack();
            StateMachine.SwitchState<AttackingState>();
            DialogueAnimator.PlayDialogueAnimation(DialogueEmotion.Exclamation);
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            combatModule.enabled = false;
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();

            CharacterData.OnAttackEnter -= () => HealthSystem.MakeInvincible(true);
            CharacterData.OnAttackExit -= () => HealthSystem.MakeInvincible(false);
        }
    }
}