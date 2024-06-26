namespace CGames
{
    public class AirAttackingState : AirborneState
    {
        public AirAttackingState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetSingleAnimation
            (
                StateClip,
                Character.CharacterAttributes.GetAttackConfig(AttackType.Airborne).CombinedAttackTimeInSeconds,
                ChangeToFallingState
            );
        }

        private void ChangeToFallingState()
        {
            StateSwitcher.SwitchState<FallingState>();
        }

        public override void Update() { }
    }
}