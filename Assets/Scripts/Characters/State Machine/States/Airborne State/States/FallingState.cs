using UnityEngine;

namespace CGames
{
    public class FallingState : AirborneState
    {
        private bool HasEnoughVelocityForMovement => Mathf.Abs(Character.Body.velocity.x) > Character.CharacterAttributes.MovementConfig.MinimumVelocityToCountAsMovement;

        public FallingState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetAnimationOnLoop(StateClip);
        }

        public override void Update()
        {
            if(Character.GroundSensor.IsGrounded == false)
                return;
            
            if(Character.CharacterData.HasHorizontalInput && HasEnoughVelocityForMovement)
                StateSwitcher.SwitchState<RunningState>();
            else
                StateSwitcher.SwitchState<IdlingState>();
        }
    }
}