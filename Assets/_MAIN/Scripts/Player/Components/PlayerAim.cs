using System;
using System.Collections;
using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;
using UnityEngine.UI;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerAim : PlayerComponent
    {
        [Header("UI")]
        [SerializeField] private bool _hideCursor = true;
        [SerializeField] private RectTransform _crosshair;
        [SerializeField] private Image _crosshairImage;
        [SerializeField] private Sprite _crosshairSprite;
        [SerializeField] private Sprite _aimCrosshairSprite;

        [Header("Aim Settings")]
        [SerializeField] private float _aimTransitionSpeed = 5f;
        [SerializeField] private float _aimSensitivityMultiplier = 0.5f; 
        [SerializeField] private AnimationCurve _aimTransitionCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);

        [Header("Arms Movement")]
        [SerializeField] private Transform _armsRoot; // FP_Arms_Pistol_01_Anims
        [SerializeField] private float _aimSensitivity = 2f;
        [SerializeField] private float _maxVerticalAngle = 25f; 
        [SerializeField] private float _maxHorizontalAngle = 20f;
        [SerializeField] private float _smoothTime = 0.1f;

        private Vector2 _currentAimInput;
        private Vector2 _aimVelocity;
        private Vector3 _originalArmsRotation;

        private float _currentSensitivityMultiplier = 1f;

        public float AimProgress { get; private set; } = 0f;


        #region Unity Lifecycle

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

            UpdateCrosshairVisibility();
        }

        public override void OnEnable()
        {
            _player.OnPlayerLook += HandleLookInput;
            _player.OnPlayerAimPressed += OnAimPressed;
            _player.OnPlayerAimReleased += OnAimReleased;
        }

        public override void OnDisable()
        {
            _player.OnPlayerLook -= HandleLookInput;
            _player.OnPlayerAimPressed -= OnAimPressed;
            _player.OnPlayerAimReleased -= OnAimReleased;
        }

        public override void OnUpdate()
        {
            UpdateAimTransition();
            UpdateArmsRotation();
            UpdateCrosshairVisibility();
        }
        
        #endregion

        #region Input Handling
        
        private void OnAimPressed()
        {
            _player.SetAiming(true);
        }

        private void OnAimReleased()
        {
            _player.SetAiming(false);
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

            float effectiveSensitivity = _aimSensitivity * _currentSensitivityMultiplier;

            _currentAimInput.x = Mathf.Clamp(normalizedMousePos.x * effectiveSensitivity, -1f, 1f);
            _currentAimInput.y = Mathf.Clamp(normalizedMousePos.y * effectiveSensitivity, -1f, 1f);
        }
        
        #endregion

        #region Aim System
        
        private void UpdateAimTransition()
        {
            float targetProgress = _player.IsAiming ? 1f : 0f;
            
            AimProgress = Mathf.MoveTowards(
                AimProgress, 
                targetProgress, 
                _aimTransitionSpeed * Time.deltaTime
            );

            float curveValue = _aimTransitionCurve.Evaluate(AimProgress);
            
            _currentSensitivityMultiplier = Mathf.Lerp(
                a: 1f, 
                _aimSensitivityMultiplier, 
                curveValue
            );
        }
        
        #endregion

        #region Crosshair Management
        
        private void UpdateCrosshairVisibility()
        {
            if (_crosshairImage != null)
            {
                if (_player.IsAiming && _aimCrosshairSprite != null)
                {
                    _crosshairImage.sprite = _aimCrosshairSprite;
                }
                else if (!_player.IsAiming && _crosshairSprite != null)
                {
                    _crosshairImage.sprite = _crosshairSprite;
                }

                Color color = _crosshairImage.color;
                color.a = Mathf.Lerp(0.7f, 1f, AimProgress);
                _crosshairImage.color = color;
            }
        }
        
        #endregion

        #region Arms Movement
        
        private void UpdateArmsRotation()
        {
            if (_armsRoot == null) return;

            _currentAimInput = Vector2.SmoothDamp(
                _currentAimInput, 
                _currentAimInput, 
                ref _aimVelocity,
                 _smoothTime
            );

            float verticalRotation = -_currentAimInput.y * _maxVerticalAngle; 
            float horizontalRotation = _currentAimInput.x * _maxHorizontalAngle;

            Vector3 targetRotation = _originalArmsRotation + new Vector3(verticalRotation, horizontalRotation, 0f);
            
            _armsRoot.localRotation = Quaternion.Euler(targetRotation);
        }
        
        #endregion

        #region Public Methods
        
        public bool TryGetAimPoint(out Vector3 aimPoint, float maxDistance)
        {
            var cam = Camera.main;

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
        #endregion
    }
}