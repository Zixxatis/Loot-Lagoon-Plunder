using System;
using UnityEngine;
using Zenject;

namespace CGames
{
    public class PlayerInputHandler : IInitializable, IDisposable//, ITickable
    {
        private static LLPInputActions inputActions;

        public static event Action OnSwappedToEnablePlayerScheme;
        public static event Action OnSwappedToDisablePlayerScheme;
        
        public static event Action OnAttackButtonPressed;
        public static event Action OnJumpButtonPressed;
        public static event Action OnInteractButtonPressed;

        private static Func<bool> shouldDisplayCursorInThisScene;
        
        public static Vector2 PlayerInput => inputActions.Player.Move.ReadValue<Vector2>();

        public PlayerInputHandler(ConfigDataView configDataView, SceneLoader sceneLoader)
        {
            inputActions = new LLPInputActions();

            shouldDisplayCursorInThisScene = () => configDataView.ShouldDisplayCursor || sceneLoader.CurrentScene == GameScene.MainMenu;
        }

        public void Initialize()
        {
            SubscribeToButtons();
            MatchCursorPreference();
        }

        private static void SubscribeToButtons()
        {
            inputActions.Player.Attack.started += (context) => OnAttackButtonPressed?.Invoke();
            inputActions.Player.Jump.started += (context) => OnJumpButtonPressed?.Invoke();
            inputActions.Player.Interact.started += (context) => OnInteractButtonPressed?.Invoke();

            inputActions.UI.Pause.started += (context) => ChangePlayerControlsStatus();
        }

    #region Input Action
        public static void EnableInputActions() => inputActions.Enable();
        public static void DisableInputActions() => inputActions.Disable();
    #endregion

    #region Schemes
        public static void ChangePlayerControlsStatus()
        {
            if(inputActions.Player.enabled)
            {
                DisablePlayerControls();
                OnSwappedToDisablePlayerScheme?.Invoke();
            }
            else
            {
                EnablePlayerControls();
                OnSwappedToEnablePlayerScheme?.Invoke();
            }
        }

        public static void EnablePlayerControls() => inputActions.Player.Enable();
        public static void DisablePlayerControls() => inputActions.Player.Disable();
    #endregion

    #region Direct Commands / Button
        public static void EnableAttack()
        {
            if(inputActions.Player.enabled)
                inputActions.Player.Attack.Enable();
        }

        public static void DisableAttack() => inputActions.Player.Attack.Disable();

        public static void EnableJump()
        {
            if(inputActions.Player.enabled)
                inputActions.Player.Jump.Enable();
        }

        public static void DisableJump() => inputActions.Player.Jump.Disable();

        public static void EnableInteraction()
        {
            if(inputActions.Player.enabled)
                inputActions.Player.Interact.Enable();
        }

        public static void DisableInteraction() => inputActions.Player.Interact.Disable();

        public static void EnablePauseListener() => inputActions.UI.Pause.Enable();
        public static void DisablePauseListener() => inputActions.UI.Pause.Disable();
    #endregion

    #region Button Listeners

    #endregion

    #region Cursor
        /// <summary> Sets cursor visibility to preference in Config. </summary>
        /// <remarks> Doesn't affect visibility in the Main Menu. </remarks> 
        public static void MatchCursorPreference()
        {
            if(shouldDisplayCursorInThisScene())
                DisplayCursor();
            else
                HideCursor();
        }

        public static void DisplayCursor()
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
        public static void HideCursor()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    #endregion

        public void Dispose()
        {
            DisableInputActions();

            inputActions.Player.Attack.started -= (context) => OnAttackButtonPressed?.Invoke();
            inputActions.Player.Jump.started -= (context) => OnJumpButtonPressed?.Invoke();
            inputActions.Player.Interact.started -= (context) => OnInteractButtonPressed?.Invoke();
            
            inputActions.UI.Pause.started -= (context) => ChangePlayerControlsStatus();
        }
    }
}