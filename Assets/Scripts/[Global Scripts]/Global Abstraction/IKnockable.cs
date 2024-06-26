using UnityEngine;

namespace CGames
{
    public interface IKnockable
    {
        public void KnockBack(Vector2 knockBackForce, bool shouldKnockBackToLeft);
    }
}