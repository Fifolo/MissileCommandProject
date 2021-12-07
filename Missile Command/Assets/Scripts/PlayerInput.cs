using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace MissileCommand
{
    [DisallowMultipleComponent]
    public class PlayerInput : MonoBehaviour
    {
        #region Variables
        [SerializeField] private Camera mainCamera;
        public delegate void FireEvent(Vector2 inputPosition);
        public event FireEvent OnFirePressed;
        private PlayerActions inputActions;
        #endregion

        #region MonoBehaviour
        private void Awake()
        {
            inputActions = new PlayerActions();
            if (mainCamera == null) Debug.LogError($"Main camera not attached to {GetType()}");
        }

        private void SubscribeToRoundEvents()
        {
            RoundManager.OnRoundFinish += OnRoundFinish;
            RoundManager.OnRoundStart += OnRoundStart;
        }
        private void UnsubscribeToRoundEvents()
        {
            RoundManager.OnRoundFinish -= OnRoundFinish;
            RoundManager.OnRoundStart -= OnRoundStart;
        }

        private void OnRoundStart(int roundNumber) => inputActions.InGame.Enable();

        private void OnRoundFinish(int roundNumber) => inputActions.InGame.Disable();

        private void OnEnable()
        {
            inputActions.InGame.Enable();
            SubscribeToInputActions();
            SubscribeToRoundEvents();
        }

        private void OnDisable()
        {
            inputActions.InGame.Disable();
            UnsubscribeToInputActions();
            UnsubscribeToRoundEvents();
        }
        #endregion

        #region Private Methods
        private void SubscribeToInputActions()
        {
            inputActions.InGame.Fire.performed += Fire_performed;
            inputActions.InGame.Pause.performed += Pause_performed;
        }

        private void UnsubscribeToInputActions()
        {
            inputActions.InGame.Fire.performed -= Fire_performed;
            inputActions.InGame.Pause.performed -= Pause_performed;
        }

        private void Pause_performed(InputAction.CallbackContext obj)
        {
            if (GameManager.Instance)
                GameManager.Instance.PauseToggle();
        }
        private void Fire_performed(InputAction.CallbackContext ctx)
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

            OnFirePressed?.Invoke(worldPosition);
        }
        #endregion
    }
}
