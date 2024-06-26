using System;
using UnityEngine;

namespace CGames
{
    /// <summary> In this task, the enemy will run from the given point until he faces the wall. </summary>
    public class RunAwayTask : EnemyTask
    {
        private readonly Func<Vector3> getChaserPosition;

        public RunAwayTask(EnemyNPC enemyNPC, Func<Vector3> getChaserPosition) : base(enemyNPC)
        {
            this.getChaserPosition = getChaserPosition;
        }
        
        public override void Update() 
        {
            if(getChaserPosition().x < EnemyNPC.transform.position.x)
                EnemyNPC.CharacterData.SetNewInput(new(1, 0));
            else
                EnemyNPC.CharacterData.SetNewInput(new(-1, 0));

            if(EnemyNPC.WallsDetector.IsNearWall)
                MarkAsCompleted();
        }

        protected override void FinishTask()
        {
            base.FinishTask();
            EnemyNPC.CharacterData.ResetInput();
        }
    }
}