namespace CGames
{
    public class AttackingState : GroundedState
    {
        public AttackingState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }
       
        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetSingleAnimation
            (
                StateClip,
                Character.CharacterAttributes.GetAttackConfig(AttackType.Default).CombinedAttackTimeInSeconds,
                ReturnToGroundedState
            );
        }

        private void ReturnToGroundedState()
        {
            if(Character.GroundSensor.IsGrounded)
                StateSwitcher.SwitchState<IdlingState>();
            else
                StateSwitcher.SwitchState<FallingState>();
        }
    }
}