using System.Collections.Generic;
using UnityEngine;
using Zenject;

namespace CGames
{
    [RequireComponent(typeof(Collider2D))]
    public abstract class InteractableObject : MonoBehaviour, ILocalizable
    {
        [Header("Interactable Object - General")]
        [SerializeField] private LocalizationKeyField infoLocalizationKeyField;
        [Space]
        [SerializeField] private GameObject interactivityIconObject;

        public bool IsPlayerInZone { get; protected set; }

        protected GoalTracker GoalTracker { get; private set; }
        protected abstract bool ShouldHandleCursorVisibility { get; }


        [Inject]
        private void Construct(GoalTracker goalTracker)
        {
            this.GoalTracker = goalTracker;
        }

        protected virtual void Awake() => interactivityIconObject.DeactivateObject();

        protected void InteractWithObject()
        {
            PlayerInputHandler.DisablePlayerControls();
            PlayerInputHandler.DisablePauseListener();

            if(ShouldHandleCursorVisibility)
                PlayerInputHandler.DisplayCursor();

            Interact();
        }

        protected abstract void Interact();

        protected void RevertInteractionWithObject()
        {
            PlayerInputHandler.EnablePlayerControls();
            PlayerInputHandler.EnablePauseListener();

            if(ShouldHandleCursorVisibility)
                PlayerInputHandler.MatchCursorPreference();

            RevertInteraction();
        }

        protected virtual void RevertInteraction() { }

        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            if(IsPlayerInZone)
                return;

            GoalTracker.SetTaskText(infoLocalizationKeyField.LocalizationKey);
            GoalTracker.ShowTask();
            interactivityIconObject.ActivateObject();

            IsPlayerInZone = true;
            EnablePlayerInteraction();
        }

        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            if(IsPlayerInZone == false)
                return;

            GoalTracker.HideTask();
            interactivityIconObject.DeactivateObject();

            IsPlayerInZone = false;
            DisablePlayerInteraction();
        }

        private void EnablePlayerInteraction()
        {
            PlayerInputHandler.OnInteractButtonPressed += InteractWithObject;
            PlayerInputHandler.EnableInteraction();
        }

        protected void DisablePlayerInteraction()
        {
            PlayerInputHandler.DisableInteraction();
            PlayerInputHandler.OnInteractButtonPressed -= InteractWithObject;
        }

        List<LocalizationKeyField> ILocalizable.GetAllLocalizationKeys() => new() { infoLocalizationKeyField };
    }
}