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

        private void OnEnable()
        {
            inputActions.InGame.Enable();
            SubscribeToInputActions();
        }

        private void OnDisable()
        {
            inputActions.InGame.Disable();
            UnsubscribeToInputActions();
        }
        #endregion

        #region Methods
        private void SubscribeToInputActions() => inputActions.InGame.Fire.performed += Fire_performed;
        private void UnsubscribeToInputActions() => inputActions.InGame.Fire.performed -= Fire_performed;
        private void Fire_performed(InputAction.CallbackContext ctx)
        {
            Vector2 screenPosition = Mouse.current.position.ReadValue();
            Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);

            OnFirePressed?.Invoke(worldPosition);
        }
        #endregion
    }
}
