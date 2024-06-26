using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CGames.VisualFX
{
    public static class FadeVFX
    {   
        public const float FullAlpha = 1f; 
        public const float NoAlpha = 0f; 

        /// <summary> Will infinitely fade graphic's alpha from full to zero, wait and then do the opposite. </summary>
        /// <remarks> Uses scaled time. Doesn't have an ending point - you have to disable this manually. </remarks>
        public static IEnumerator FadeInfinitely(Graphic objectToFade, float fadeEffectDuration, float inBetweenAwaitDuration)
        {
            if(objectToFade.color.a > NoAlpha)
            {
                yield return ChangeAlpha(objectToFade, NoAlpha, fadeEffectDuration);
                yield return new WaitForSeconds(inBetweenAwaitDuration);
            }

            while (true)
            {
                yield return ChangeAlpha(objectToFade, FullAlpha, fadeEffectDuration);
                yield return new WaitForSeconds(inBetweenAwaitDuration);

                yield return ChangeAlpha(objectToFade, NoAlpha, fadeEffectDuration);
                yield return new WaitForSeconds(inBetweenAwaitDuration);
            }
        }

        /// <summary> Will infinitely fade graphic's alpha from full to zero, wait and then do the opposite. </summary>
        /// <remarks> Uses unscaled time. Doesn't have an ending point - you have to disable this manually. </remarks>
        public static IEnumerator FadeInfinitelyUnscaled(Graphic objectToFade, float fadeEffectDuration, float inBetweenAwaitDuration)
        {
            if(objectToFade.color.a > NoAlpha)
            {
                yield return ChangeAlphaUnscaled(objectToFade, NoAlpha, fadeEffectDuration);
                yield return new WaitForSecondsRealtime(inBetweenAwaitDuration);
            }

            while (true)
            {
                yield return ChangeAlphaUnscaled(objectToFade, FullAlpha, fadeEffectDuration);
                yield return new WaitForSecondsRealtime(inBetweenAwaitDuration);

                yield return ChangeAlphaUnscaled(objectToFade, NoAlpha, fadeEffectDuration);
                yield return new WaitForSecondsRealtime(inBetweenAwaitDuration);
            }
        }

        /// <summary> Will start a coroutine that will change graphic's alpha to the target alpha in given time. </summary>
        /// <remarks> Uses scaled delta time. </remarks>
        public static IEnumerator ChangeAlpha(Graphic objectToFade, float targetAlpha, float animationDuration)
        {
            return ChangeAlphaOverTime(objectToFade, targetAlpha, animationDuration, true);
        }

        /// <summary> Will start a coroutine that will change graphic's alpha to the target alpha in given time. </summary>
        /// <remarks> Uses unscaled delta time. </remarks>
        public static IEnumerator ChangeAlphaUnscaled(Graphic objectToFade, float targetAlpha, float animationDuration)
        {
            return ChangeAlphaOverTime(objectToFade, targetAlpha, animationDuration, false);
        }

        private static IEnumerator ChangeAlphaOverTime(Graphic objectToFade, float targetAlpha, float animationDuration, bool shouldUseScaledTime)
        {
            bool ShouldChangeAlpha() => Mathf.Abs(objectToFade.color.a - targetAlpha) > 0.01f;
            float GetTime() => shouldUseScaledTime? Time.deltaTime : Time.unscaledDeltaTime;

            while (ShouldChangeAlpha())
            {
                float newAlpha = Mathf.MoveTowards(objectToFade.color.a, targetAlpha, GetTime() / animationDuration);
                SetAlpha(objectToFade, newAlpha);

                yield return null;
            }

            SetAlpha(objectToFade, targetAlpha);
        }

        private static void SetAlpha(Graphic graphic, float newAlpha) => graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, newAlpha);
    }
}