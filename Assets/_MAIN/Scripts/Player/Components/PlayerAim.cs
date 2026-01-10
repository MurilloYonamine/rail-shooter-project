using System;
using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerAim : PlayerComponent
    {
        private InputReader _inputReader;
        [SerializeField] private bool _hideCursor = true;
        [SerializeField] private float _sensitivity = 2.0f;

        [SerializeField] private RectTransform _crosshair;

        public override void OnAwake()
        {
            _inputReader = new InputReader();
        }
        public override void OnStart()
        {
            if (_hideCursor)
            {
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
            }
        }
        public override void OnEnable()
        {
            _inputReader.OnEnable();
            InputReader.OnLookInput += HandleLookInput;
        }
        public override void OnDisable()
        {
            _inputReader.OnDisable();
            InputReader.OnLookInput -= HandleLookInput;
        }
        private void HandleLookInput(Vector2 lookInput)
        {
            float mouseX = lookInput.x * _sensitivity;
            float mouseY = lookInput.y * _sensitivity;

            (Vector2 minBounds, Vector2 maxBounds) = GetScreenBounds();

            Vector3 currentPosition = _crosshair.anchoredPosition;
            currentPosition += new Vector3(mouseX, mouseY, 0);

            currentPosition.x = Mathf.Clamp(currentPosition.x, minBounds.x, maxBounds.x);
            currentPosition.y = Mathf.Clamp(currentPosition.y, minBounds.y, maxBounds.y);

            _crosshair.anchoredPosition = currentPosition;
        }
        public Vector3 GetAimWorldPosition(float distance)
        {
            Camera cam = Camera.main;

            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 crosshairScreenPos = screenCenter + _crosshair.anchoredPosition;

            Ray ray = cam.ScreenPointToRay(crosshairScreenPos);
            return ray.origin + ray.direction * distance;
        }
        private static (Vector2, Vector2) GetScreenBounds()
        {
            int screenWidth = Screen.width;
            int screenHeight = Screen.height;

            Vector2 minBounds = new Vector2(-screenWidth / 2f, -screenHeight / 2f);
            Vector2 maxBounds = new Vector2(screenWidth / 2f, screenHeight / 2f);

            return (minBounds, maxBounds);
        }
    }
}