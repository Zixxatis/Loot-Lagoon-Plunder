using System.Collections;
using CGames.VisualFX;
using UnityEngine;

namespace CGames
{
    public class Barrel : DestroyableObstacle
    {
        [Header("Barrel Settings")]
        [SerializeField] private float delayDuration = 0.1f;
        [SerializeField] private float fallingDuration = 0.15f;
        [Space]
        [SerializeField] private float fallHeight = -0.25f;

        private void OnEnable() => OnObjectHit += DropParticles;
        public void OnDisable() => OnObjectHit -= DropParticles;

        private void DropParticles() => StartCoroutine(DropParticlesAfterDelay());

        private IEnumerator DropParticlesAfterDelay()
        {
            yield return new WaitForSeconds(delayDuration);

            yield return StartCoroutine(TransformVFX.MoveObject(this.transform, new(0, fallHeight, 0), fallingDuration));

            Destroy(this.gameObject);
        }
    }
}