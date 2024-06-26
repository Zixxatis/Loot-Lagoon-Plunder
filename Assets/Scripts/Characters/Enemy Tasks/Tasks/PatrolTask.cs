using UnityEngine;

namespace CGames
{   
    /// <summary> In this task, the enemy will run forward until he reaches a wall or obstacle. </summary>
    /// <remarks> If the task is successfully completed - will change direction. </remarks>
    public class PatrolTask : EnemyTask
    {   
        private const float AwaitCapInSeconds = 1f;
        private float awaitTimer;
        private Vector2 lastPosition;

        public PatrolTask(EnemyNPC enemyNPC) : base(enemyNPC)
        {
        }

        public override void StartTask()
        {
            base.StartTask();

            awaitTimer = 0;
            lastPosition = Vector2.zero;
        }

        public override void Update()
        {
            EnemyNPC.CharacterData.SetNewInput(new(EnemyNPC.Body.transform.localScale.x, 0));

            if(lastPosition == CurrentPosition)
                awaitTimer += Time.deltaTime;

            lastPosition = CurrentPosition;

            if(EnemyNPC.WallsDetector.IsNearWall || awaitTimer > AwaitCapInSeconds)
                MarkAsCompleted();
        }

        protected override void OnTaskComplete()
        {   
            base.OnTaskComplete();
            EnemyNPC.ChangeDirection();
        }

        protected override void FinishTask()
        {
            base.FinishTask();
            EnemyNPC.CharacterData.ResetInput();
        }

        private Vector2 CurrentPosition => EnemyNPC.Body.transform.position;
    }
}