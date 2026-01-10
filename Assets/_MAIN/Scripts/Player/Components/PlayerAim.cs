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
                Cursor.lockState = CursorLockMode.Confined;
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
            Vector2 mouseScreenPos = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 relativePosition = mouseScreenPos - screenCenter;

            _crosshair.anchoredPosition = relativePosition;
        }

        public bool TryGetAimPoint(out Vector3 aimPoint, float maxDistance)
        {
            Camera cam = Camera.main;

            Vector2 mouseScreenPos = Input.mousePosition;

            Ray ray = cam.ScreenPointToRay(mouseScreenPos);

            if (Physics.Raycast(ray, out RaycastHit hit, maxDistance))
            {
                aimPoint = hit.point;
                return true;
            }

            aimPoint = ray.origin + ray.direction * maxDistance;
            return false;
        }
    }
}
