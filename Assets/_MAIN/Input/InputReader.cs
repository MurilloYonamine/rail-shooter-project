using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RAIL_SHOOTER.PLAYER.INPUT
{
    public class InputReader
    {
        private readonly PlayerControls _playerControls;
        private readonly InputAction _look;
        private readonly InputAction _fire;
        private readonly InputAction _aim;
        private readonly InputAction _reload;

        public static Action<Vector2> OnLookInput;

        public static Action OnAimPressed;
        public static Action OnAimReleased;

        public static Action OnFirePressed;
        public static Action OnFireReleased;

        public static Action OnReloadPressed;

        public InputReader()
        {
            _playerControls = new PlayerControls();
            _look = _playerControls.Player.Look;
            _fire = _playerControls.Player.Fire;
            _aim = _playerControls.Player.Aim;
            _reload = _playerControls.Player.Reload;
        }
        public void OnEnable()
        {
            _playerControls.Enable();

            _look.Enable();
            _look.performed += HandleLookInput;

            _fire.Enable();
            _fire.performed += HandleAttackInput;
            _fire.canceled += HandleAttackInput;

            _aim.Enable();
            _aim.performed += HandleAimInput;
            _aim.canceled += HandleAimInput;

            _reload.Enable();
            _reload.performed += HandleReloadInput;
        }
        public void OnDisable()
        {
            _playerControls.Disable();

            _look.Disable();
            _look.performed -= HandleLookInput;

            _fire.Disable();
            _fire.performed -= HandleAttackInput;
            _fire.canceled -= HandleAttackInput;

            _aim.Disable();
            _aim.performed -= HandleAimInput;
            _aim.canceled -= HandleAimInput;

            _reload.Disable();
            _reload.performed -= HandleReloadInput;
        }
        private void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = _look.ReadValue<Vector2>();
            OnLookInput?.Invoke(lookInput);
        }
        private void HandleAttackInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnFirePressed?.Invoke();
            }
            else if (context.canceled)
            {
                OnFireReleased?.Invoke();
            }
        }
        private void HandleAimInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnAimPressed?.Invoke();
            }
            else if (context.canceled)
            {
                OnAimReleased?.Invoke();
            }
        }
        private void HandleReloadInput(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                OnReloadPressed?.Invoke();
            }
        }
    }
}