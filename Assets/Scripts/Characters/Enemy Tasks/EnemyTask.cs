namespace CGames
{
    public abstract class EnemyTask : ITask
    {   
        protected readonly EnemyNPC EnemyNPC;

        private bool isFinished;
        private bool isCompleted;

        protected EnemyTask(EnemyNPC enemyNPC)
        {
            EnemyNPC = enemyNPC;
        }

        /// <summary> A preparation before executing task. </summary>
        public virtual void StartTask()
        {
            isFinished = false;
            isCompleted = false;
        }

        /// <summary> A task itself. Completes until something else happens. </summary>
        /// <remarks> Should have an exit point to 'MarkAsCompleted'. </remarks>
        public abstract void Update();


        /// <summary> Marks task as completed and does actions from 'OnTaskComplete'. After that - finishes the task. </summary>
        protected void MarkAsCompleted() 
        {
            isCompleted = true;

            OnTaskComplete();
            FinishTask();
        }
        
        /// <summary> Optional method, that will be invoked after task is completed. </summary>
        /// <remarks> Won't be called if the task is aborted. </remarks>
        protected virtual void OnTaskComplete() { }

        /// <summary> A finishing method that will be called in both cases: if the task is completed, or it gets aborted. </summary>
        protected virtual void FinishTask()
        {
            isFinished = true;
        }

        /// <summary> Forcefully finished the task. </summary>
        /// <remarks> Won't call the 'OnTaskComplete' method. </remarks>
        public void ForceToAbortTask() => FinishTask();

        bool ITask.IsFinished() => isFinished;
        bool ITask.IsCompleted() => isCompleted;

    }
}