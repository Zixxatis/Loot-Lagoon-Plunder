namespace CGames
{
    public class HitState : ActionState
    {
        public HitState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetSingleAnimation
            (
                StateClip,
                Character.CharacterAttributes.HealthSystemConfig.RecoveryWindowInSeconds,
                ReturnToNormalState
            );
        }

        private void ReturnToNormalState()
        {
            if (Character.GroundSensor.IsGrounded)
                StateSwitcher.SwitchState<IdlingState>();
            else
                StateSwitcher.SwitchState<FallingState>();
        }

        public override void Update() { }
    }
}