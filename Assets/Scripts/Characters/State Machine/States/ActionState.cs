namespace CGames
{
    public abstract class ActionState : IState
    {
        protected readonly IStateSwitcher StateSwitcher;
        protected readonly Character Character;

        protected StateClip StateClip { get; private set; }

        public ActionState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip)
        {
            StateSwitcher = stateSwitcher;
            Character = character;
            StateClip = stateClip;
        }

        public virtual void Enter()
        {
            //UnityEngine.Debug.Log("Entered:" + GetType()); 
        }

        public virtual void Exit()
        {
            //UnityEngine.Debug.LogWarning("Exited:" + GetType());
            StateClip.StopSoundEffect();
        }

        public abstract void Update();
    }
}