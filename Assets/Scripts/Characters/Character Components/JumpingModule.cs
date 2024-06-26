using UnityEngine;

namespace CGames
{
    public class JumpingModule : MonoBehaviour
    {
        private Rigidbody2D body;
        private GroundSensor groundSensor;

        private AirborneConfig airborneConfig;

        public void Initialize(Character character)
        {
            this.body = character.Body;
            this.groundSensor = character.GroundSensor; 

            this.airborneConfig  = character.CharacterAttributes.AirborneConfig;
        }

        public void Jump()
        {
            if(groundSensor.IsGrounded == false)
                return;

            Vector2 newVelocity = new(body.velocity.x, airborneConfig.JumpForce);
            body.velocity = newVelocity;
        }
    }
}