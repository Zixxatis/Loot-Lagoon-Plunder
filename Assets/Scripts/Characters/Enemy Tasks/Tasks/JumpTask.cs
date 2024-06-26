using System;
using UnityEngine;

namespace CGames
{
    /// <summary> In this task, the enemy will use given 'jumpAction' and wait for landing. </summary>
    /// <remarks> If the task is successfully completed & if facing the wall - will stop, change direction and continue to move. </remarks>
    public class JumpTask : EnemyTask
    {
        private readonly Action jumpAction;

        private const float DelayBeforeDetectingFloor = 0.1f;
        private float timer;

        public JumpTask(EnemyNPC enemyNPC, Action jumpAction) : base(enemyNPC)
        {
            this.jumpAction = jumpAction;
        }

        public override void StartTask()
        {
            base.StartTask();

            timer = 0f;
            jumpAction?.Invoke();
        }

        public override void Update()
        {
            if(timer < DelayBeforeDetectingFloor)
            {
                timer += Time.deltaTime;
                return;
            }
            
            if(EnemyNPC.GroundSensor.IsGrounded)
                MarkAsCompleted();
        }

        protected override void OnTaskComplete()
        {
            base.OnTaskComplete();

            if(EnemyNPC.WallsDetector.IsNearWall)
            {
                EnemyNPC.CharacterData.ResetInput();
                EnemyNPC.ChangeDirection();
                EnemyNPC.CharacterData.SetNewInput(new(EnemyNPC.Body.transform.localScale.x, 0));
            }
        }
    }
}