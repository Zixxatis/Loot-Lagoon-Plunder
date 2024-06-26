using System;
using UnityEngine;

namespace CGames
{
    /// <summary> In this task, the enemy will run towards given target until he's attack range allows it to reaches the target. After that he attacks. </summary>
    public class ApproachToAttackTask : EnemyTask
    {
        private readonly Action attackAction;
        private readonly Func<Vector3> getAttackPointPosition;
        private readonly Func<Vector3> getTargetPosition;

        private readonly float attackRangeRadius;
        private bool haveTriedToAttack;

        public ApproachToAttackTask(EnemyNPC enemyNPC, Action attackAction, Func<Vector3> getAttackPointPosition, Func<Vector3> getTargetPosition) : base(enemyNPC)
        {
            this.attackAction = attackAction;
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
                HandleMovement();

                if (CanReachTargetWithAttack)
                {
                    EnemyNPC.CharacterData.ResetInput();

                    attackAction?.Invoke();
                    haveTriedToAttack = true;
                }

                return;
            }

            if (EnemyNPC.CharacterData.IsAttacking == false && haveTriedToAttack)
                MarkAsCompleted();
        }

        private void HandleMovement()
        {
            if (IsNearTarget)
                EnemyNPC.CharacterData.ResetInput();
            else
            {
                if (getTargetPosition().x < EnemyNPC.transform.position.x)
                    EnemyNPC.CharacterData.SetNewInput(new(-1, 0));
                else
                    EnemyNPC.CharacterData.SetNewInput(new(1, 0));
            }
        }

        private bool IsNearTarget => Mathf.Abs(getTargetPosition().x - EnemyNPC.transform.position.x) <= attackRangeRadius * 2;
        private bool CanReachTargetWithAttack => (Mathf.Abs(getTargetPosition().x - getAttackPointPosition().x) <= attackRangeRadius * 2)
                                                 && Mathf.Abs(getTargetPosition().y - getAttackPointPosition().y) <= attackRangeRadius * 2;
    }
}