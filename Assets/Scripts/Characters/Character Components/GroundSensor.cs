using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class GroundSensor : MonoBehaviour
    {
        [Header("Layer Masks")]
        [SerializeField] private LayerMask touchableLayers;
        [SerializeField] private LayerMask dangerousLayers;

        public bool IsGrounded { get; private set; }
        public bool IsTouchingHazards { get; private set; }
        public bool IsTouchingGroundOrHazards => IsGrounded || IsTouchingHazards;

        private BoxCollider2D feetCollider;

        public void Initialize()
        {
            feetCollider = GetComponent<BoxCollider2D>();
        }

        private void FixedUpdate()
        {
            CheckGround();
            CheckHazards();
        }

        private void CheckGround() => IsGrounded = feetCollider.IsTouchingLayers(touchableLayers);
        private void CheckHazards() => IsTouchingHazards = feetCollider.IsTouchingLayers(dangerousLayers);
    }
}