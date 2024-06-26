namespace CGames
{
    public abstract class AirborneState : ActionState
    {
        protected AirborneState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }
        public override void Enter()
        {
            base.Enter();

            Character.CharacterData.EnterAirborne();
        }

        public override abstract void Update();

        public override void Exit()
        {
            base.Exit();

            Character.CharacterData.ExitAirborne();
        }
    }
}