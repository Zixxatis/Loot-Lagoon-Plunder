using System;

namespace CGames
{
    /// <summary> In this task, the enemy will use the given 'AttackAction' and wait for 'IsAttacking' to end. </summary>
    public class AttackTask : EnemyTask
    {
        private readonly Action attackAction;
        private readonly bool shouldContinueMoving;

        public AttackTask(EnemyNPC enemyNPC, Action attackAction, bool shouldContinueMoving = true) : base(enemyNPC)
        {
            this.attackAction = attackAction;
            this.shouldContinueMoving = shouldContinueMoving;
        }

        public override void StartTask()
        {
            base.StartTask();

            attackAction?.Invoke();
        }

        public override void Update()
        {
            if(shouldContinueMoving)
                EnemyNPC.CharacterData.SetNewInput(new(EnemyNPC.Body.transform.localScale.x, 0));

            if(EnemyNPC.CharacterData.IsAttacking == false)
                MarkAsCompleted();
        }

        protected override void FinishTask()
        {
            base.FinishTask();

            EnemyNPC.CharacterData.ResetInput();
        }
    }
}