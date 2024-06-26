using UnityEngine;

namespace CGames
{
    public class DeathState : ActionState
    {
        public DeathState(IStateSwitcher stateSwitcher, Character character, StateClip stateClip) : base(stateSwitcher, character, stateClip)
        {
        }

        public override void Enter()
        {
            base.Enter();

            Character.CharacterAnimator.SetAnimationOnLoop(StateClip);
        }

        public override void Update()
        {
            while(Character.GroundSensor.IsTouchingGroundOrHazards == false)
            {
                return;
            }

            Character.Body.velocity = Vector2.zero;
        }
    }
}