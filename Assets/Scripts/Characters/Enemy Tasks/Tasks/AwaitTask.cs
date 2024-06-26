using UnityEngine;

namespace CGames
{
    /// <summary> In this task, the enemy will stand still for a given amount of time. </summary>
    public class AwaitTask : EnemyTask
    {
        private readonly float awaitDuration;
        private float timer;

        public AwaitTask(EnemyNPC enemyNPC, float awaitDuration) : base(enemyNPC)
        {
            this.awaitDuration = awaitDuration;
        }

        public override void StartTask()
        {
            base.StartTask();

            EnemyNPC.Body.velocity = new(0, 0);

            timer = 0f;
        }

        public override void Update()
        {
            timer += Time.deltaTime;
            
            if(timer >= awaitDuration)
                MarkAsCompleted();
        }

        protected override void FinishTask()
        {   
            base.FinishTask();

            timer = 0f;
        }
    }
}