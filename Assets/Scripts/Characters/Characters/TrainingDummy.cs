using System.Collections.Generic;
using UnityEngine;

namespace CGames
{
    public class TrainingDummy : Character
    {
        [Header("Training Dummy State Clips")]
        [SerializeField] private StateClip idlingStateClip;
        [SerializeField] private StateClip fallingStateClip;
        [Space]
        [SerializeField] private StateClip hitStateClip;

        private float startingPositionX;

        protected override CharacterAttributes GetCharacterAttributes(CharacterConfig characterConfig) => new CharacterAttributes(characterConfig);

        protected override List<IState> GetCharacterStates()
        {
            return new()
            {
                new IdlingState(StateMachine, this, idlingStateClip),
                new FallingState(StateMachine, this, fallingStateClip),
                new HitState(StateMachine, this, hitStateClip)
            };
        }

        protected override void InitializeCharacter() 
        {
            startingPositionX = this.transform.position.x;
        }

        protected override void Update()
        {
            base.Update();

            if(HealthSystem.HasFullHP == false)
                HealthSystem.SetToFullHP();

            if(transform.position.x != startingPositionX)
                transform.position = new(startingPositionX, transform.position.y);
            
            Body.velocity = Vector2.zero;
        }
    }
}