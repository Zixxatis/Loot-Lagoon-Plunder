using System;
using System.Collections;
using UnityEngine;
using TMPro;

namespace CGames
{
    public class NumericPopupField : MonoBehaviour
    {	
        [SerializeField] private TextMeshProUGUI textMeshProUGUI;
        [Space]
        [SerializeField] private float lifeTime = 0.75f;

        [Header("Colors")]
        [SerializeField] private Color normalDamageColor = new(1, 1, 1, 0);
        [SerializeField] private Color criticalDamageColor = new(1, 0, 0, 0);
        [SerializeField] private Color enemyDamageColor = new(1, 1, 1, 0);
        [Space]
        [SerializeField] private Color healColor = new(0, 1, 0, 0);
        
        [Header("Animation Curves")]
        [SerializeField] private AnimationCurve opacityCurve;
        [Space]   
        [SerializeField] private AnimationCurve scaleCurve;
        [SerializeField] private AnimationCurve criticalScaleCurve;
        [Space]
        [SerializeField] private AnimationCurve heightCurve;

        private Action returnToPollAction;

        public void Initialize(Action returnToPollAction)
        {
            this.returnToPollAction = returnToPollAction;
        }

        public void ActivateEnemyDamagePopup(int damageAmount, bool isCritical)
        {
            StartCoroutine( DisplayPopup
            (
                isCritical? $"*{damageAmount}*" : damageAmount.ToString(),
                isCritical? criticalDamageColor : enemyDamageColor, 
                isCritical? criticalScaleCurve : scaleCurve
            ));
        }

        public void ActivatePlayerDamagePopup(int damageAmount, bool isCritical)
        {
            StartCoroutine( DisplayPopup
            (
                isCritical? $"*{damageAmount}*" : damageAmount.ToString(),
                isCritical? criticalDamageColor : normalDamageColor, 
                isCritical? criticalScaleCurve : scaleCurve
            ));
        }

        public void ActivateHealPopup(int healAmount) => StartCoroutine(DisplayPopup($"+ {healAmount}", healColor, scaleCurve));

        private IEnumerator DisplayPopup(string damageText, Color targetColor, AnimationCurve targetCurve)
        {
            Vector3 originalPosition = transform.position;
            float timer = 0f;

            textMeshProUGUI.text = damageText;

            while (timer < lifeTime)
            {
                textMeshProUGUI.color = new Color(targetColor.r, targetColor.g, targetColor.b, opacityCurve.Evaluate(timer / lifeTime));
                transform.localScale = Vector3.one * targetCurve.Evaluate(timer / lifeTime);
                transform.position = originalPosition + new Vector3(0, 1 + heightCurve.Evaluate(timer / lifeTime), 0);

                timer += Time.deltaTime;

                yield return null;
            }
            
            returnToPollAction?.Invoke();
        }
    }
}