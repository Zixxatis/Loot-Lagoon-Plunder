namespace CGames
{
    public interface ITask
    {
        public void StartTask();
        public void Update();
        public void ForceToAbortTask();

        public bool IsFinished();
        public bool IsCompleted();
    }
}