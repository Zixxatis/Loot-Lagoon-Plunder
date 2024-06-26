using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Collider2D))]
    public class HazardObject : MonoBehaviour
    {
        [SerializeField, Min(1)] private int damageAmount;
        [Space]
        [SerializeField] private Vector2 knockBackForce;

        private void OnCollisionEnter2D(Collision2D collision)
        {          
            IDamageable damagedObject = collision.gameObject.transform.GetComponentInChildren<IDamageable>();

            damagedObject.TakeDamage(damageAmount);

            if(damagedObject is IKnockable knockableObject)
            {
                bool isPlayerAtLeftSide = collision.transform.position.x < this.transform.position.x ;
                knockableObject.KnockBack(knockBackForce, isPlayerAtLeftSide);
            }
        }
    }
}