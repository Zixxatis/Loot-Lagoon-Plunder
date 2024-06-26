using CGames.Utilities;
using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Collider2D))]
    public class TouchContactDetector : MonoBehaviour
    {   
        private int damage;
        private Vector2 knockBackForce;

        public void Initialize(TouchKnockBackConfig touchKnockBackConfig)
        {
            damage = touchKnockBackConfig.Damage;
            knockBackForce = touchKnockBackConfig.KnockBackForce;
        }

        public void Initialize(int damage, Vector2 knockBackForce)
        {
            this.damage = damage;
            this.knockBackForce = knockBackForce;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if(collision.gameObject.CompareTag(LayerUtilities.PlayerTag) == false)
                return;

            IDamageable damagedObject = collision.gameObject.transform.GetComponentInChildren<IDamageable>();
            damagedObject.TakeDamage(damage);

            if(damagedObject is IKnockable knockableObject)
            {
                bool isPlayerAtLeftSide = collision.transform.position.x < this.GetComponentInParent<Character>().transform.position.x;
                knockableObject.KnockBack(knockBackForce, isPlayerAtLeftSide);
            }
        }
    }
}