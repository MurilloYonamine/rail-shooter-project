using System;
using RAIL_SHOOTER.AUDIO;
using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Components")] private PlayerComponent[] _playerComponents;
        [SerializeField] private PlayerAim _playerAim;
        [SerializeField] private PlayerMovement _playerMovement;
        [SerializeField] private PlayerGun _playerGun;
        [SerializeField] private PlayerAnimator _playerAnimator;
        [SerializeField] private PlayerAudio _playerAudio;

        private InputReader _inputReader;

        public event Action<Vector2> OnPlayerLook;
        public event Action OnPlayerFirePressed;
        public event Action OnPlayerFireReleased;
        public event Action OnPlayerAimPressed;
        public event Action OnPlayerAimReleased;
        public event Action OnPlayerReloadPressed;

        [Header("Player States")] [SerializeField]
        private float _shootCooldown = 0.5f;

        [SerializeField] private bool _isReloading;

        public bool IsReloading => _isReloading;
        private bool IsFiring { get; set; }
        public bool IsAiming { get; private set; }

        public bool CanShoot => Time.time >= LastShootTime + _shootCooldown;
        private bool CanPerformAction => !_isReloading && !IsFiring;
        public bool CanFire => CanPerformAction && CanShoot;

        public float LastShootTime { get; set; }
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
                _playerGun,
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
            _inputReader = new InputReader();
            _inputReader.OnEnable();

            _inputReader.OnLookInput += HandleLookInput;
            _inputReader.OnFirePressed += HandleFirePressed;
            _inputReader.OnFireReleased += HandleFireReleased;
            _inputReader.OnAimPressed += HandleAimPressed;
            _inputReader.OnAimReleased += HandleAimReleased;
            _inputReader.OnReloadPressed += HandleReloadPressed;

            ForEachComponent(component => component?.OnEnable());
        }

        private void OnDisable()
        {
            if (_inputReader == null) return;
            
            _inputReader.OnDisable();

            _inputReader.OnLookInput -= HandleLookInput;
            _inputReader.OnFirePressed -= HandleFirePressed;
            _inputReader.OnFireReleased -= HandleFireReleased;
            _inputReader.OnAimPressed -= HandleAimPressed;
            _inputReader.OnAimReleased -= HandleAimReleased;
            _inputReader.OnReloadPressed -= HandleReloadPressed;

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
            foreach (var component in _playerComponents)
            {
                componentAction(component);
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
        public void SetFiring(bool value) => IsFiring = value;
        public void SetAiming(bool value) => IsAiming = value;

        #endregion

        #region Player Components Getters

        public PlayerAim PlayerAim => _playerAim;
        public PlayerMovement PlayerMovement => _playerMovement;
        public PlayerGun PlayerGun => _playerGun;
        public PlayerAnimator PlayerAnimator => _playerAnimator;
        public PlayerAudio PlayerAudio => _playerAudio;

        #endregion
    }
}