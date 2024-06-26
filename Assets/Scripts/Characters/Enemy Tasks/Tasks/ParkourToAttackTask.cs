using System;
using UnityEngine;

namespace CGames
{
    /// <summary> In this task, the enemy will run towards given target until he's attack range allows it to reaches the target. After that he attacks. </summary>
    /// <remarks> Can jump, if located near wall. </remarks>
    public class ParkourToAttackTask : EnemyTask
    {
        private readonly Action attackAction;
        private readonly Action jumpAction;
        private readonly Func<Vector3> getAttackPointPosition;
        private readonly Func<Vector3> getTargetPosition;

        private readonly float attackRangeRadius;
        private bool haveTriedToAttack;

        public ParkourToAttackTask(EnemyNPC enemyNPC, Action attackAction, Action jumpAction, Func<Vector3> getAttackPointPosition, Func<Vector3> getTargetPosition) : base(enemyNPC)
        {
            this.attackAction = attackAction;
            this.jumpAction = jumpAction;
            this.getAttackPointPosition = getAttackPointPosition;
            this.getTargetPosition = getTargetPosition;

            attackRangeRadius = enemyNPC.CharacterAttributes.GetAttackConfig(AttackType.Default).AttackRangeRadius;
        }

        public override void StartTask()
        {
            base.StartTask();

            haveTriedToAttack = false;
        }

        public override void Update()
        {
            if(haveTriedToAttack == false)
            {
                if (ShouldJump)
                {
                    jumpAction?.Invoke();
                    return;
                }
                
                HandleMovement();

                if (CanReachTargetWithAttack)
                    Attack();

                return;
            }

            if (EnemyNPC.CharacterData.IsAttacking == false && haveTriedToAttack)
                MarkAsCompleted();
        }

        private void HandleMovement()
        {
            if (getTargetPosition().x < EnemyNPC.transform.position.x)
                EnemyNPC.CharacterData.SetNewInput(new(-1, 0));
            else
                EnemyNPC.CharacterData.SetNewInput(new(1, 0));
        }

        private void Attack()
        {
            EnemyNPC.CharacterData.ResetInput();

            attackAction?.Invoke();
            haveTriedToAttack = true;
        }

        private bool ShouldJump => EnemyNPC.WallsDetector.IsNearWall && EnemyNPC.GroundSensor.IsGrounded;
        private bool CanReachTargetWithAttack => (Mathf.Abs(getTargetPosition().x - getAttackPointPosition().x) <= attackRangeRadius * 2)
                                                 && Mathf.Abs(getTargetPosition().y - getAttackPointPosition().y) <= attackRangeRadius * 2;
    }
}