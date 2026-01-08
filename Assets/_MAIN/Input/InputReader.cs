using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RAIL_SHOOTER.PLAYER.INPUT
{
    public class InputReader
    {
        private readonly PlayerControls _playerControls;
        private readonly InputAction _look;
        private readonly InputAction _attack;

        public static Action<Vector2> OnLookInput;

        public InputReader()
        {
            _playerControls = new PlayerControls();
            _look = _playerControls.Player.Look;
            _attack = _playerControls.Player.Attack;
        }
        public void OnEnable()
        {
            _playerControls.Enable();

            _look.Enable();
            _look.performed += HandleLookInput;

            _attack.Enable();
            _attack.performed += HandleAttackInput;
        }
        public void OnDisable()
        {
            _playerControls.Disable();

            _look.Disable();
            _look.performed -= HandleLookInput;

            _attack.Disable();
            _attack.performed -= HandleAttackInput;
        }
        private void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = _look.ReadValue<Vector2>();
            OnLookInput?.Invoke(lookInput);
        }
        private void HandleAttackInput(InputAction.CallbackContext context)
        {
            Debug.Log("Attack Input Performed");
        }
    }
}