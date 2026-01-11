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
        private InputReader _inputReader;

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
        
        private bool _isAiming = false;
        private float _aimTransitionProgress = 0f;
        private float _currentSensitivityMultiplier = 1f;

        public bool IsAiming => _isAiming;
        public float AimProgress => _aimTransitionProgress;
        

        #region Unity Lifecycle
        
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

            UpdateCrosshairVisibility();
        }

        public override void OnEnable()
        {
            _inputReader.OnEnable();
            InputReader.OnLookInput += HandleLookInput;
            InputReader.OnAimPressed += OnAimPressed;
            InputReader.OnAimReleased += OnAimReleased;
        }

        public override void OnDisable()
        {
            _inputReader.OnDisable();
            InputReader.OnLookInput -= HandleLookInput;
            InputReader.OnAimPressed -= OnAimPressed;
            InputReader.OnAimReleased -= OnAimReleased;
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
            _isAiming = true;
        }

        private void OnAimReleased()
        {
            _isAiming = false;
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
            float targetProgress = _isAiming ? 1f : 0f;
            
            _aimTransitionProgress = Mathf.MoveTowards(
                _aimTransitionProgress, 
                targetProgress, 
                _aimTransitionSpeed * Time.deltaTime
            );

            float curveValue = _aimTransitionCurve.Evaluate(_aimTransitionProgress);
            
            _currentSensitivityMultiplier = Mathf.Lerp(1f, _aimSensitivityMultiplier, curveValue);
        }
        
        #endregion

        #region Crosshair Management
        
        private void UpdateCrosshairVisibility()
        {
            if (_crosshairImage != null)
            {
                if (_isAiming && _aimCrosshairSprite != null)
                {
                    _crosshairImage.sprite = _aimCrosshairSprite;
                }
                else if (!_isAiming && _crosshairSprite != null)
                {
                    _crosshairImage.sprite = _crosshairSprite;
                }

                Color color = _crosshairImage.color;
                color.a = Mathf.Lerp(0.7f, 1f, _aimTransitionProgress);
                _crosshairImage.color = color;
            }
        }
        
        #endregion

        #region Arms Movement
        
        private void UpdateArmsRotation()
        {
            if (_armsRoot == null) return;

            _currentAimInput = Vector2.SmoothDamp(_currentAimInput, _currentAimInput, ref _aimVelocity, _smoothTime);

            float verticalRotation = -_currentAimInput.y * _maxVerticalAngle; 
            float horizontalRotation = _currentAimInput.x * _maxHorizontalAngle;

            Vector3 targetRotation = _originalArmsRotation + new Vector3(verticalRotation, horizontalRotation, 0f);
            
            _armsRoot.localRotation = Quaternion.Euler(targetRotation);
        }
        
        #endregion

        #region Public Methods
        
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

        public float GetAccuracyMultiplier()
        {
            return Mathf.Lerp(0.7f, 1f, _aimTransitionProgress);
        }

        public void SetAimTransitionSpeed(float speed)
        {
            _aimTransitionSpeed = Mathf.Clamp(speed, 0.1f, 20f);
        }

        public void SetAimSensitivityMultiplier(float multiplier)
        {
            _aimSensitivityMultiplier = Mathf.Clamp(multiplier, 0.1f, 2f);
        }
        
        #endregion
    }
}