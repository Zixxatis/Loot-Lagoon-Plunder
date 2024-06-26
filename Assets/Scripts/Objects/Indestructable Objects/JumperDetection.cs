using System;
using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Collider2D))]
    public class JumperDetection : MonoBehaviour
    {
        public event Action OnJump;

        private void OnCollisionEnter2D(Collision2D collision)
        {
            OnJump?.Invoke();
        }
    }
}