using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace RAIL_SHOOTER.PLAYER.INPUT
{
    public class InputReader
    {
        private readonly PlayerControls m_playerControls;
        private readonly InputAction m_look;
        private readonly InputAction m_attack;

        public static Action<Vector2> OnLookInput;

        public InputReader()
        {
            m_playerControls = new PlayerControls();
            m_look = m_playerControls.Player.Look;
            m_attack = m_playerControls.Player.Attack;
        }
        public void OnEnable()
        {
            m_playerControls.Enable();

            m_look.Enable();
            m_look.performed += HandleLookInput;

            m_attack.Enable();
            m_attack.performed += HandleAttackInput;
        }
        public void OnDisable()
        {
            m_playerControls.Disable();

            m_look.Disable();
            m_look.performed -= HandleLookInput;

            m_attack.Disable();
            m_attack.performed -= HandleAttackInput;
        }
        private void HandleLookInput(InputAction.CallbackContext context)
        {
            Vector2 lookInput = m_look.ReadValue<Vector2>();
            OnLookInput?.Invoke(lookInput);
        }
        private void HandleAttackInput(InputAction.CallbackContext context)
        {
            Debug.Log("Attack Input Performed");
        }
    }
}