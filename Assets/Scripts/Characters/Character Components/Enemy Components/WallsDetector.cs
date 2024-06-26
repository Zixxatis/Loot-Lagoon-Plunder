using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(BoxCollider2D))]
    public class WallsDetector : MonoBehaviour
    {
        [Header("Layer Masks")]
        [SerializeField] private LayerMask wallsLayers;

        public bool IsNearWall { get; private set; }
        private BoxCollider2D detectorCollider;

        public void Initialize() => detectorCollider = GetComponent<BoxCollider2D>();

        private void FixedUpdate()
        {
            IsNearWall = detectorCollider.IsTouchingLayers();
        }
    }
}