using UnityEngine;

namespace CGames
{
    /// <summary> In this task, the enemy will run towards given point until he reaches it. </summary>
    public class WanderTask : EnemyTask
    {
        private const float PermissibleZone = 0.05f;
        private const float MinimumDistanceDifference = 2f;

        private readonly Transform leftPoint;
        private readonly Transform rightPoint;
        private readonly bool shouldIgnoreSmartRandom;

        private float targetPositionX;

        public WanderTask(EnemyNPC enemyNPC, Transform leftPoint, Transform rightPoint) : base(enemyNPC)
        {
            if(leftPoint.position.x < rightPoint.position.x)
            {
                this.leftPoint = leftPoint;
                this.rightPoint = rightPoint;
            }
            else
            {
                this.leftPoint = rightPoint;
                this.rightPoint = leftPoint;
            }

            if((rightPoint.position.x - leftPoint.position.x) / 2 < MinimumDistanceDifference)
            {
                shouldIgnoreSmartRandom = true;
                Debug.LogWarning("The distance between two points is too short! Won't use \"smart\" target point calculations.");
            }
        }

        public override void StartTask()
        {
            base.StartTask();

            FindNewTargetPoint();

            if (targetPositionX < EnemyNPC.transform.position.x)
                EnemyNPC.CharacterData.SetNewInput(new(-1, 0));
            else
                EnemyNPC.CharacterData.SetNewInput(new(1, 0));
        }
        
        public override void Update()
        {
            if (IsNearTarget)
                MarkAsCompleted();
        }

        protected override void FinishTask()
        {
            base.FinishTask();
            EnemyNPC.CharacterData.ResetInput();
        }
        
        private void FindNewTargetPoint()
        {
            if(shouldIgnoreSmartRandom)
            {
                targetPositionX = Random.Range(leftPoint.position.x, rightPoint.position.x);
                return;
            }

            float previousTargetPositionX = targetPositionX;

            do
                targetPositionX = Random.Range(leftPoint.position.x, rightPoint.position.x);
            while
                (Mathf.Abs(previousTargetPositionX - targetPositionX) < MinimumDistanceDifference);
        }


        private bool IsNearTarget => targetPositionX - PermissibleZone < EnemyNPC.transform.position.x && EnemyNPC.transform.position.x < targetPositionX + PermissibleZone;
    }
}