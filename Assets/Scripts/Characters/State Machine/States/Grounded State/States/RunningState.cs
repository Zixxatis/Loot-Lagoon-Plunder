using UnityEngine;

namespace CGames
{
    public class RunningState : GroundedState
    {
        private bool HasEnoughVelocityForMovement => Mathf.Abs(Character.Body.velocity.x) > Character.CharacterAttributes.MovementConfig.MinimumVelocityToCountAsMovement;

        public RunningState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
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
            
            if(Character.CharacterData.HasHorizontalInput == false || HasEnoughVelocityForMovement == false)
                StateSwitcher.SwitchState<IdlingState>();
        }
    }
}