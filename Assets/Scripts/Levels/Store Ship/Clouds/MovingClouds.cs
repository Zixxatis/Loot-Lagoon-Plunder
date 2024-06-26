using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(SpriteRenderer))]
    public class MovingClouds : MonoBehaviour
    {
        [SerializeField] private Transform startingPoint;
        [SerializeField] private Transform finishingPoint;
        [Space]
        [SerializeField, Min(0.1f)] private float speed;

        private Transform Transform { get; set; }
        private Vector3 Position => this.transform.position;

        private void Awake() => Transform = this.transform;

        private void Update()
        {
            if(Position.x < finishingPoint.position.x)
                ReturnToStartingPoint();
            else
                Transform.Translate(speed * Time.deltaTime * Vector3.left);
        }

        private void ReturnToStartingPoint() => Transform.position = new Vector3(startingPoint.position.x, Position.y, Position.z);
    }
}