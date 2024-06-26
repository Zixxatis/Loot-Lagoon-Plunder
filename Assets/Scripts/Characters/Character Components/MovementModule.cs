using UnityEngine;

namespace CGames
{
    public class MovementModule : MonoBehaviour
    {
        private const float FrictionThreshold = 0.01f;

        private Rigidbody2D body;
        private GroundSensor groundSensor;

        private MovementConfig movementConfig;
        private CharacterData characterData;

        public void Initialize(Character character)
        {
            this.body = character.Body;
            this.groundSensor = character.GroundSensor; 

            this.movementConfig = character.CharacterAttributes.MovementConfig;
            this.characterData = character.CharacterData;
        }

        private void FixedUpdate() 
        {
            HandleMovement();
            ApplyFriction();
        }

        private void HandleMovement() 
        {
            if(characterData.HasHorizontalInput) 
            {
                float accelerationForce = characterData.InputVector.x * movementConfig.Acceleration;
                float newVelocityX = Mathf.Clamp(body.velocity.x + accelerationForce, -movementConfig.MaxSpeedX, movementConfig.MaxSpeedX);

                body.velocity = new Vector2(newVelocityX, body.velocity.y);

                HandleDirection();
            }
        }

        private void HandleDirection() 
        {
            float direction = Mathf.Sign(characterData.InputVector.x);
            body.transform.localScale = new Vector3(direction, 1, 1);
        }

        private void ApplyFriction() 
        {
            if(groundSensor.IsGrounded && characterData.HasHorizontalInput == false) 
            {
                if(Mathf.Abs(body.velocity.x) < FrictionThreshold)
                    body.velocity = new(0, body.velocity.y);
                else
                    body.velocity = new(body.velocity.x * movementConfig.InertiaMultiplier, body.velocity.y);
            }
        }
    }
}