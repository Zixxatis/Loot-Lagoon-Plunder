using UnityEngine;

namespace CGames
{
    public class IdlingState : GroundedState
    {
        private bool HasEnoughVelocityForMovement => Mathf.Abs(Character.Body.velocity.x) > Character.CharacterAttributes.MovementConfig.MinimumVelocityToCountAsMovement;

        public IdlingState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetAnimationOnLoop(StateClip);
        }

        public override void Update()
        {
            base.Update();

            if(Character.CharacterData.HasHorizontalInput && HasEnoughVelocityForMovement)
                StateSwitcher.SwitchState<RunningState>();
        }
    }
}