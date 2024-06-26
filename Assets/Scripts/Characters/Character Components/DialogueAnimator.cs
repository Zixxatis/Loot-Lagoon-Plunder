using System.Collections;
using UnityEngine;

namespace CGames
{
    [RequireComponent(typeof(Animator))]
    public class DialogueAnimator : MonoBehaviour
    {
        [SerializeField] private AnimationClip emptyDialogueClip;
        [SerializeField] private AnimationClip exclamationDialogueClip;
        [SerializeField] private AnimationClip questionDialogueClip;
        [SerializeField] private AnimationClip deathDialogueClip;

        private Animator animator;
        private AnimationClip currentAnimationClip;
        
        private bool IsAnimationPlaying => animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f;

        public void Initialize()
        {
            animator = GetComponent<Animator>();
            DisableObject();
        }

        public void PlayDialogueAnimation(DialogueEmotion dialogueEmotion) 
        {
            this.ActivateGameObject();

            if(this.gameObject.activeInHierarchy == false)
                return;

            currentAnimationClip = GetAnimationClip(dialogueEmotion);
            animator.Play(currentAnimationClip.name);

            StartCoroutine(WaitForAnimationToEnd());
        }

        private AnimationClip GetAnimationClip(DialogueEmotion dialogueEmotion)
        {
            return dialogueEmotion switch
            {
                DialogueEmotion.Exclamation => exclamationDialogueClip,
                DialogueEmotion.Question => questionDialogueClip,
                DialogueEmotion.Death => deathDialogueClip,
                _ => emptyDialogueClip,
            };
        }

        private IEnumerator WaitForAnimationToEnd()
        {
            yield return null;

            while(IsAnimationPlaying)
            {
                if(transform.parent.transform.localScale.x == -1)
                    this.transform.localEulerAngles = new(0, 180, 0);
                else
                    this.transform.localEulerAngles = new(0, 0, 0);
                                
                yield return null;
            }
            
            DisableObject();
        }
        
        private void DisableObject()
        {
            currentAnimationClip = GetAnimationClip(DialogueEmotion.Empty);
            this.DeactivateGameObject();
        }
    }

    public enum DialogueEmotion
    {
        Empty,
        Exclamation,
        Question,
        Death
    }
}