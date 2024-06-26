using UnityEngine;
using UnityEngine.UI;
using CGames.VisualFX;

namespace CGames
{
    public class GoalTracker : MonoBehaviour
    {  
        [Header("Current Task Info")]
        [SerializeField] private Image goalBackgroundImage;
        [SerializeField] private TextLocalizer goalLTMP;
        [Space]
        [SerializeField, Min(0.1f)] private float fadeInEffectDuration = 0.5f;

        private void Awake()
        {
            goalLTMP.Graphic.MakeInvisible();
            goalBackgroundImage.MakeInvisible();
        }
        
        public void SetTaskText(string taskKey) => goalLTMP.SetKeyAndUpdate(taskKey);

        public void ShowTask()
        {
            StopAllCoroutines();

            StartCoroutine(FadeVFX.ChangeAlpha(goalLTMP.Graphic, FadeVFX.FullAlpha, fadeInEffectDuration));
            StartCoroutine(FadeVFX.ChangeAlpha(goalBackgroundImage, FadeVFX.FullAlpha, fadeInEffectDuration));
        }

        public void HideTask()
        {
            StopAllCoroutines();

            if(this.gameObject.activeInHierarchy)
            {
                StartCoroutine(FadeVFX.ChangeAlpha(goalLTMP.Graphic, FadeVFX.NoAlpha, fadeInEffectDuration));
                StartCoroutine(FadeVFX.ChangeAlpha(goalBackgroundImage, FadeVFX.NoAlpha, fadeInEffectDuration));
            }
        }

        private void OnDestroy() => HideTask();
    }
}