using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class ShellNPC : EnemyNPC
    {
        [Header("Shell NPC Components")]
        [SerializeField] private ShellShooterModule shellShooterModule;

        [Header("Shell State Clips")]
        [SerializeField] private StateClip idlingStateClip;
        [SerializeField] private StateClip attackingStateClip;
        [Space]
        [SerializeField] private StateClip fallingStateClip;
        [Space]
        [SerializeField] private StateClip hitStateClip;
        [SerializeField] private StateClip deathStateClip;

        protected override float DeathAnimationDuration => deathStateClip.AnimationDuration;

        private float startingPositionX;

        protected override List<IState> GetCharacterStates()
        {
            return new()
            {
                new IdlingState(StateMachine, this, idlingStateClip),
                new AttackingState(StateMachine, this, attackingStateClip),

                new FallingState(StateMachine, this, fallingStateClip),

                new HitState(StateMachine, this, hitStateClip),
                new DeathState(StateMachine, this, deathStateClip),
            };
        }

        protected override Queue<ITask> GetDefaultTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.GeneralAwait)));

            return taskQueue;
        }

        protected override Queue<ITask> GetLongRangeTaskList()
        {
            Queue<ITask> taskQueue = new();

            taskQueue.Enqueue(new AttackTask(this, EnterAttackingState, false));
            taskQueue.Enqueue(new AwaitTask(this, EnemyAttributes.GetAwaitTime(AwaitTimingType.AfterAttackRecovery)));

            return taskQueue;
        }

        protected override void InitializeEnemy()
        {
            startingPositionX = this.transform.position.x;
            shellShooterModule.Initialize(CharacterData);
        }

        private void FixedUpdate()
        {   
            if(transform.position.x != startingPositionX)
                transform.position = new(startingPositionX, transform.position.y);

            Body.velocity = Vector2.zero;
        }

        protected override void EnterAttackingState()
        {
            shellShooterModule.Shoot();
            StateMachine.SwitchState<AttackingState>();
        }

        protected override void OnDeath()
        {
            base.OnDeath();

            shellShooterModule.enabled = false;
        }
    }
}