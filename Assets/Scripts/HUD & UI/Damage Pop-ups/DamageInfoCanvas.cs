using System.Collections;
using CGames.VisualFX;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

namespace CGames
{
    public class DamageInfoCanvas : MonoBehaviour
    {
        private static DamageInfoCanvas instance;

        [Header("Overlay FX")]
        [SerializeField] private Image overlayEffectImage;
        [SerializeField, Range(0.1f, 1f)] private float overlayMaxAlpha;
        [Space]
        [SerializeField, Min(0.05f)] private float fadeOverlayEffectDuration = 0.25f;
        [SerializeField, Min(0.05f)] private float awaitOverlayEffectDuration = 0.1f;

        [Header("Pop-up")]
        [SerializeField] private NumericPopupField numericPopupPrefab;

        private ObjectPool<NumericPopupField> popupPool;

        private void Awake()
        {
            overlayEffectImage.MakeInvisible();
            overlayEffectImage.DeactivateGameObject();
            instance = this;

            popupPool = new
            (
                CreatePopup,
                x => x.ActivateGameObject(),
                x => x.DeactivateGameObject(),
                x => Destroy(x.gameObject)
            );
        }

        private NumericPopupField CreatePopup()
        {
            NumericPopupField prefab = Instantiate(instance.numericPopupPrefab);
            prefab.Initialize(() => popupPool.Release(prefab));

            prefab.transform.SetParent(this.transform, true);

            return prefab;
        }

        public static void DisplayDamage(bool isDamageFromEnemy, int damageAmount, Vector3 position, bool isCritical = false)
        {
            NumericPopupField damagePopupField = instance.popupPool.Get();
            damagePopupField.transform.position = position;

            if (isDamageFromEnemy)
                damagePopupField.ActivateEnemyDamagePopup(damageAmount, isCritical);
            else
                damagePopupField.ActivatePlayerDamagePopup(damageAmount, isCritical);
        }

        public static void DisplayHeal(int healAmount, Vector3 position)
        {
            NumericPopupField healPopupField = instance.popupPool.Get();
            healPopupField.transform.position = position;

            healPopupField.ActivateHealPopup(healAmount);
        }

        public static void ShowOverlayEffect()
        {
            if(instance.overlayEffectImage.gameObject.activeInHierarchy)
                return;

            instance.overlayEffectImage.ActivateGameObject();
            instance.StartCoroutine(instance.FlashOverlayEffect());
        }

        private IEnumerator FlashOverlayEffect()
        {
            yield return StartCoroutine(FadeVFX.ChangeAlpha(overlayEffectImage, overlayMaxAlpha, fadeOverlayEffectDuration));
            yield return new WaitForSeconds(awaitOverlayEffectDuration);
            yield return StartCoroutine(FadeVFX.ChangeAlpha(overlayEffectImage, FadeVFX.NoAlpha, fadeOverlayEffectDuration));

            overlayEffectImage.DeactivateGameObject();
        }
    }
}