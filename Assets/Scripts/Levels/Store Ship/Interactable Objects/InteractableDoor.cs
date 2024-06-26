using System;
using System.Collections;
using UnityEngine;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Collider2D))]
    public class InteractableDoor : InteractableObject
    {
        [Header("Door Elements")]
        [SerializeField] private Animator animator;
        [Space]
        [SerializeField] private AnimationClip idleClosedClip;
        [SerializeField] private AnimationClip openingClip;
        [SerializeField] private AnimationClip idleOpenedClip;

        protected override bool ShouldHandleCursorVisibility => false;

        private Action saveDataAction;
        private Action loadRandomLevelAction;

        [Inject]
        private void Construct(PlayerDataView playerDV, SceneLoader sceneLoader)
        {
            this.saveDataAction = playerDV.SaveData;
            this.loadRandomLevelAction = sceneLoader.LoadRandomLevel;
        }

        protected override void Awake()
        {
            base.Awake();

            animator.Play(idleClosedClip.name);
        }

        protected override void Interact()
        { 
            DisablePlayerInteraction();

            IsPlayerInZone = false;
            GoalTracker.HideTask();

            saveDataAction?.Invoke();

            StartCoroutine(WaitForPlayerToLeave());
        }

        private IEnumerator WaitForPlayerToLeave()
        {
            animator.Play(openingClip.name);
            yield return new WaitForSeconds(openingClip.length);

            animator.Play(idleOpenedClip.name);
            yield return new WaitForSeconds(idleOpenedClip.length);


            loadRandomLevelAction?.Invoke();
        }
    }
}