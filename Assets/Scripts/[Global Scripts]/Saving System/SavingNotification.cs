using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using CGames.VisualFX;

namespace CGames
{
    public class SavingNotification : MonoBehaviour
    {
        public static SavingNotification Instance { get; private set; }

        [SerializeField] private Image savingIcon;
        [Space]
        [SerializeField] private float fadeTiming = 0.75f;
        [SerializeField] private float awaitDuration = 0.2f;
        [SerializeField] private int repeatsAmount = 2;

        private IEnumerator displayCoroutine;

        private void Awake()
        {
            Instance = this;

            savingIcon.MakeInvisible();
        }

        public void ShowNotification()
        {
            if(displayCoroutine == null)
            {
                displayCoroutine = ShowNotificationAnimation();
                StartCoroutine(displayCoroutine);
            }
        }

        private IEnumerator ShowNotificationAnimation()
        {
            for (int i = 0; i < repeatsAmount; i++)
            {
                yield return StartCoroutine(FadeVFX.ChangeAlphaUnscaled(savingIcon, FadeVFX.FullAlpha, fadeTiming)); 
                yield return new WaitForSecondsRealtime(awaitDuration);
                yield return StartCoroutine(FadeVFX.ChangeAlphaUnscaled(savingIcon, FadeVFX.NoAlpha, fadeTiming));
            }

            savingIcon.MakeInvisible();

            displayCoroutine = null;
        }
    }
}

