using UnityEngine;

namespace CGames
{
    public class JumpingState : AirborneState
    {
        public JumpingState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetAnimationOnLoop(StateClip);
        }

        public override void Update()
        {
            if(Character.Body.velocity.y < Mathf.Epsilon)
                StateSwitcher.SwitchState<FallingState>();
        }
    }
}