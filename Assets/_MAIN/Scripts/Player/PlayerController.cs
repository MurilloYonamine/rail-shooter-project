using System;
using RAIL_SHOOTER.AUDIO;
using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")]
        private PlayerComponent[] _playerComponents;
        [SerializeField] private PlayerAim _playerAim;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerShoot _playerShoot;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAudio _playerAudio;

        private InputReader _inputReader;

        public event Action<Vector2> OnPlayerLook;
        public event Action OnPlayerFirePressed;
        public event Action OnPlayerFireReleased;
        public event Action OnPlayerAimPressed;
        public event Action OnPlayerAimReleased;
        public event Action OnPlayerReloadPressed;

        [Header("Player States")]
        [SerializeField] private float _shootCooldown = 0.5f;
        private float _lastShootTime;
        private bool _isReloading;
        private bool _isFiring;
        private bool _isAiming;

        public bool IsReloading => _isReloading;
        public bool IsFiring => _isFiring;
        public bool IsAiming => _isAiming;
        public bool CanShoot => Time.time >= _lastShootTime + _shootCooldown;
        public bool CanPerformAction => !_isReloading && !_isFiring;
        public bool CanFire => CanPerformAction && CanShoot;

        public float LastShootTime { get => _lastShootTime; set => _lastShootTime = value; }
        public float ShootCooldown => _shootCooldown;

        [SerializeField] private AudioClip _environmentSound;

        #region Unity Cycle Methods
        private void Awake()
        {
            _inputReader = new InputReader();
            _playerComponents = new PlayerComponent[]
            {
                _playerAim,
                _playerMovement,
                _playerShoot,
                _playerAudio,
                _playerAnimator
            };
            ForEachComponent(component => component?.Initialize(this));
            ForEachComponent(component => component?.OnAwake());
        }

        private void Start()
        {
            ForEachComponent(component => component?.OnStart());
            AudioManager.Instance.PlayEnvironmentSFX(_environmentSound, transform.position, volume: 0.2f);
        }

        private void OnEnable()
        {
            _inputReader.OnEnable();

            InputReader.OnLookInput += HandleLookInput;
            InputReader.OnFirePressed += HandleFirePressed;
            InputReader.OnFireReleased += HandleFireReleased;
            InputReader.OnAimPressed += HandleAimPressed;
            InputReader.OnAimReleased += HandleAimReleased;
            InputReader.OnReloadPressed += HandleReloadPressed;

            ForEachComponent(component => component?.OnEnable());
        }

        private void OnDisable()
        {
            _inputReader.OnDisable();

            InputReader.OnLookInput -= HandleLookInput;
            InputReader.OnFirePressed -= HandleFirePressed;
            InputReader.OnFireReleased -= HandleFireReleased;
            InputReader.OnAimPressed -= HandleAimPressed;
            InputReader.OnAimReleased -= HandleAimReleased;
            InputReader.OnReloadPressed -= HandleReloadPressed;

            ForEachComponent(component => component?.OnDisable());
        }

        private void Update()
        {
            ForEachComponent(component => component?.OnUpdate());
        }

        private void FixedUpdate()
        {
            ForEachComponent(component => component?.OnFixedUpdate());
        }

        private void LateUpdate()
        {
            ForEachComponent(component => component?.OnLateUpdate());
        }

        private void ForEachComponent(Action<PlayerComponent> componentAction)
        {
            for (int i = 0; i < _playerComponents.Length; i++)
            {
                componentAction(_playerComponents[i]);
            }
        }
        #endregion

        #region Input Handlers

        private void HandleLookInput(Vector2 lookInput)
        {
            OnPlayerLook?.Invoke(lookInput);
        }

        private void HandleFirePressed()
        {
            OnPlayerFirePressed?.Invoke();
        }

        private void HandleFireReleased()
        {
            OnPlayerFireReleased?.Invoke();
        }

        private void HandleAimPressed()
        {
            OnPlayerAimPressed?.Invoke();
        }

        private void HandleAimReleased()
        {
            OnPlayerAimReleased?.Invoke();
        }

        private void HandleReloadPressed()
        {
            OnPlayerReloadPressed?.Invoke();
        }

        public void SetReloading(bool value) => _isReloading = value;
        public void SetFiring(bool value) => _isFiring = value;
        public void SetAiming(bool value) => _isAiming = value;

        #endregion

        #region Player Components Getters
        public PlayerAim PlayerAim => _playerAim;
        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerShoot PlayerShoot => _playerShoot;
        public PlayerAnimator PlayerAnimator => _playerAnimator;
        public PlayerAudio PlayerAudio => _playerAudio;

        #endregion
    }
}