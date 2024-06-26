namespace CGames
{
    public class ChargeRunTask : EnemyTask
    {
        /// <summary> In this task, the enemy will run forward until he reaches a wall or obstacle. </summary>
        /// <remarks> Won't decrease speed and change direction upon completing </remarks>
        public ChargeRunTask(EnemyNPC enemyNPC) : base(enemyNPC)
        {
        }

        public override void Update()
        {
            EnemyNPC.CharacterData.SetNewInput(new(EnemyNPC.Body.transform.localScale.x, 0));

            if(EnemyNPC.WallsDetector.IsNearWall)
                MarkAsCompleted();
        }
    }
}