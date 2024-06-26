namespace CGames
{
    public abstract class GroundedState : ActionState
    {
        protected GroundedState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }
        
        public override void Update()
        {
            if(Character.GroundSensor.IsGrounded)
                return;

            StateSwitcher.SwitchState<FallingState>();
        }
    }
}