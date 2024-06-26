using UnityEngine;
using UnityEngine.UI;
using CGames.VisualFX;

namespace CGames
{
    public class NavigationArrow : MonoBehaviour
    {
        [Header("Finishing Arrow")]
        [SerializeField] private RectTransform arrowRT;
        [SerializeField] private Image arrowImage;
        [Space]
        [SerializeField, Min(0.1f)] private float fadeInEffectDuration = 0.5f;

        private Transform targetTransform;
        private Camera mainCamera;

        private void Awake()
        {
            mainCamera = Camera.main;

            arrowImage.MakeInvisible();
            arrowRT.DeactivateGameObject();
        }

        private void Update()
        {
            if(arrowRT.gameObject.activeInHierarchy)
            {
                Vector3 direction = targetTransform.position - mainCamera.transform.position;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

                arrowRT.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
            }
        }
        
        public void SetTargetTransform(Transform targetTransform) => this.targetTransform = targetTransform;

        public void DisplayArrow()
        {
            arrowRT.ActivateGameObject();
            StartCoroutine(FadeVFX.ChangeAlpha(arrowImage, FadeVFX.FullAlpha, fadeInEffectDuration));
        }

        public void HideArrow() => StartCoroutine(FadeVFX.ChangeAlpha(arrowImage, FadeVFX.NoAlpha, fadeInEffectDuration));
    }
}