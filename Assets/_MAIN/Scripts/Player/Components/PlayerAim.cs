using System;
using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerAim : PlayerComponent
    {
        private InputReader _inputReader;

        [Header("UI")]
        [SerializeField] private bool _hideCursor = true;
        [SerializeField] private RectTransform _crosshair;

        [Header("Arms Movement")]
        [SerializeField] private Transform _armsRoot; // FP_Arms_Pistol_01_Anims
        [SerializeField] private float _aimSensitivity = 2f;
        [SerializeField] private float _maxVerticalAngle = 25f; 
        [SerializeField] private float _maxHorizontalAngle = 20f;
        [SerializeField] private float _smoothTime = 0.1f;

        // ...existing code...
        private Vector2 _currentAimInput;
        private Vector2 _aimVelocity;
        private Vector3 _originalArmsRotation;

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

            if (_armsRoot != null)
            {
                _originalArmsRotation = _armsRoot.localEulerAngles;
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

        public override void OnUpdate()
        {
            UpdateArmsRotation();
        }

        private void HandleLookInput(Vector2 lookInput)
        {
            Vector2 mouseScreenPos = Input.mousePosition;
            Vector2 screenCenter = new Vector2(Screen.width / 2f, Screen.height / 2f);
            Vector2 relativePosition = mouseScreenPos - screenCenter;

            _crosshair.anchoredPosition = relativePosition;

            Vector2 normalizedMousePos = new Vector2(
                (mouseScreenPos.x / Screen.width - 0.5f) * 2f,
                (mouseScreenPos.y / Screen.height - 0.5f) * 2f
            );

            _currentAimInput.x = Mathf.Clamp(normalizedMousePos.x * _aimSensitivity, -1f, 1f);
            _currentAimInput.y = Mathf.Clamp(normalizedMousePos.y * _aimSensitivity, -1f, 1f);
        }

        private void UpdateArmsRotation()
        {
            if (_armsRoot == null) return;

            _currentAimInput = Vector2.SmoothDamp(_currentAimInput, _currentAimInput, ref _aimVelocity, _smoothTime);

            float verticalRotation = -_currentAimInput.y * _maxVerticalAngle; 
            float horizontalRotation = _currentAimInput.x * _maxHorizontalAngle;

            Vector3 targetRotation = _originalArmsRotation + new Vector3(verticalRotation, horizontalRotation, 0f);
            
            _armsRoot.localRotation = Quaternion.Euler(targetRotation);
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

        public void ResetArmsRotation()
        {
            if (_armsRoot != null)
            {
                _armsRoot.localRotation = Quaternion.Euler(_originalArmsRotation);
            }
            _currentAimInput = Vector2.zero;
        }

        public void SetAimIntensity(float intensity)
        {
            _aimSensitivity = Mathf.Clamp(intensity, 0f, 5f);
        }
    }
}