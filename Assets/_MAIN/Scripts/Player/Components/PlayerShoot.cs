using System;
using System.Collections;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerShoot : PlayerComponent
    {
        public enum GunType
        {
            Pistol = 0,
            Rifle,
            Shotgun
        }

        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootForce = 700f;
        [SerializeField] private float _range = 100f;
        
        [Header("Ammunition")]
        [SerializeField] private int _maxAmmo = 6;
        [SerializeField] private float _reloadTime = 2f;
        private int _currentAmmo;
        private bool _isReloading;

        public int CurrentAmmo => _currentAmmo;
        public int MaxAmmo => _maxAmmo;
        public bool IsReloading => _isReloading;
        public bool HasAmmo => _currentAmmo > 0;

        public event Action OnAmmoChanged;
        public event Action OnReloadStarted;
        public event Action OnReloadFinished;
        public event Action OnShootSuccessful; 
        public event Action OnShootFailed;
        
        [Header("Accuracy")]
        [SerializeField] private float _hipFireSpread = 5f; 
        [SerializeField] private float _aimSpread = 1f; 

        public override void OnStart()
        {
            _currentAmmo = _maxAmmo;
            OnAmmoChanged?.Invoke();
        }

        public override void OnEnable()
        {
            _player.OnPlayerFirePressed += OnFirePressed;
            _player.OnPlayerFireReleased += OnFireReleased;
            _player.OnPlayerReloadPressed += OnReloadPressed;
        }

        public override void OnDisable()
        {
            _player.OnPlayerFirePressed -= OnFirePressed;
            _player.OnPlayerFireReleased -= OnFireReleased;
            _player.OnPlayerReloadPressed -= OnReloadPressed;
        }

        private void OnFirePressed()
        {
            if (HasAmmo && !_isReloading)
            {
                Shoot();
                OnShootSuccessful?.Invoke();
            }
            else if (!HasAmmo && !_isReloading)
            {
                OnShootFailed?.Invoke(); 
                StartReload();
            }
        }

        private void OnFireReleased()
        {
        }

        private void OnReloadPressed()
        {
            if (!_isReloading && _currentAmmo < _maxAmmo)
            {
                StartReload();
            }
        }

        private void Shoot()
        {
            if (!_player.PlayerAim.TryGetAimPoint(out Vector3 aimPoint, _range))
            {
                return;
            }

            Vector3 shootDirection = (aimPoint - _shootPoint.position).normalized;
            
            float spreadAngle = _player.IsAiming ? _aimSpread : _hipFireSpread;
            shootDirection = ApplySpread(shootDirection, spreadAngle);

            Debug.DrawRay(_shootPoint.position, shootDirection * 10f, Color.red, 1f);

            Quaternion rotationOffset = Quaternion.Euler(0f, 90f, 0f);
            Quaternion bulletRotation = Quaternion.LookRotation(shootDirection) * rotationOffset;

            PlayerBullet bullet = BulletPool.Instance.GetBullet();
            bullet.transform.SetPositionAndRotation(_shootPoint.position, bulletRotation);
            bullet.transform.parent = null;

            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();
            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;
            rigidBody.AddForce(shootDirection * _shootForce, ForceMode.Impulse);

            _player.LastShootTime = Time.time;

            _currentAmmo--;
            OnAmmoChanged?.Invoke();

            if (_currentAmmo <= 0)
            {
                StartReload();
            }
        }

        private void StartReload()
        {
            if (!_isReloading)
            {
                _player.StartCoroutine(ReloadCoroutine());
            }
        }

        private IEnumerator ReloadCoroutine()
        {
            _isReloading = true;
            _player.SetReloading(true);
            OnReloadStarted?.Invoke();

            yield return new WaitForSeconds(_reloadTime);

            _currentAmmo = _maxAmmo;
            _isReloading = false;
            _player.SetReloading(false);
            OnAmmoChanged?.Invoke();
            OnReloadFinished?.Invoke();
        }
        
        private Vector3 ApplySpread(Vector3 direction, float spreadAngle)
        {
            float spreadRad = spreadAngle * Mathf.Deg2Rad;
            
            float randomX = UnityEngine.Random.Range(-spreadRad, spreadRad);
            float randomY = UnityEngine.Random.Range(-spreadRad, spreadRad);
            
            Quaternion spreadRotation = Quaternion.Euler(
                randomY * Mathf.Rad2Deg, 
                randomX * Mathf.Rad2Deg, 
                z: 0
            );
            return spreadRotation * direction;
        }
    }
}
