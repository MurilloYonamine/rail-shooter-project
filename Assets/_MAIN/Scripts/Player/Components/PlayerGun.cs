using System;
using System.Collections;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerGun : PlayerComponent
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

        [Header("Ammunition")] [SerializeField]
        private int _maxAmmo = 6;

        [SerializeField] private float _reloadTime = 2f;

        public int CurrentAmmo { get; private set; }
        public bool IsReloading { get; private set; }

        public int MaxAmmo => _maxAmmo;
        public bool HasAmmo => CurrentAmmo > 0;

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
            CurrentAmmo = _maxAmmo;
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
            switch (HasAmmo)
            {
                case true when !IsReloading && _player.CanShoot:
                    Shoot();
                    OnShootSuccessful?.Invoke();
                    break;
                case false when !IsReloading:
                    OnShootFailed?.Invoke();
                    StartReload();
                    break;
            }
        }

        private void OnFireReleased()
        {
        }

        private void OnReloadPressed()
        {
            if (!IsReloading && CurrentAmmo < _maxAmmo)
            {
                StartReload();
            }
        }

        #region Shooting Logic

        private void Shoot()
        {
            TryGetAimPoint(out Vector3 aimPoint);

            Vector3 shootDirection = GetShootDirection(aimPoint);

            float spreadAngle = _player.IsAiming ? _aimSpread : _hipFireSpread;
            shootDirection = ApplySpread(shootDirection, spreadAngle);

            Debug.DrawRay(_shootPoint.position, shootDirection * 10f, Color.red, 1f);

            var bulletRotation = GetBulletRotation(shootDirection);

            var bullet = SpawnBullet(bulletRotation);

            var rigidBody = bullet.GetComponent<Rigidbody>();
            ResetRigidbody(rigidBody);
            rigidBody.AddForce(shootDirection * _shootForce, ForceMode.Impulse);

            _player.LastShootTime = Time.time;

            CurrentAmmo--;
            OnAmmoChanged?.Invoke();

            if (CurrentAmmo <= 0)
            {
                RequestReload();
            }
        }

        private bool TryGetAimPoint(out Vector3 aimPoint)
        {
            return _player.PlayerAim.TryGetAimPoint(out aimPoint, _range);
        }

        private Vector3 GetShootDirection(Vector3 aimPoint)
        {
            return (aimPoint - _shootPoint.position).normalized;
        }

        private Quaternion GetBulletRotation(Vector3 direction)
        {
            var rotationOffset = Quaternion.Euler(0f, 90f, 0f);
            return Quaternion.LookRotation(direction) * rotationOffset;
        }

        private PlayerBullet SpawnBullet(Quaternion rotation)
        {
            var bullet = BulletPool.Instance.GetBullet();
            bullet.transform.SetPositionAndRotation(_shootPoint.position, rotation);
            bullet.transform.parent = null;
            return bullet;
        }

        private void ResetRigidbody(Rigidbody rb)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        #endregion

        #region Reloading Logic
        private void RequestReload()
        {
            if (IsReloading) return;
            _player.StartCoroutine(ReloadNextFrame());
        }
        private IEnumerator ReloadNextFrame()
        {
            yield return null;
            StartReload();
        }
        private void StartReload()
        {
            if (IsReloading) return;
            
            IsReloading = true;
            _player.SetReloading(true);
            
            OnReloadStarted?.Invoke();

            _player.StartCoroutine(ReloadCoroutine());
        }

        private IEnumerator ReloadCoroutine()
        {
            yield return new WaitForSeconds(_reloadTime);
            FinishReload();
        }

        private void FinishReload()
        {
            CurrentAmmo = _maxAmmo;
            
            IsReloading = false;
            _player.SetReloading(IsReloading);
            
            OnReloadFinished?.Invoke();
            OnAmmoChanged?.Invoke();
        }

        #endregion

        private Vector3 ApplySpread(Vector3 direction, float spreadAngle)
        {
            float spreadRad = spreadAngle * Mathf.Deg2Rad;

            float randomX = UnityEngine.Random.Range(-spreadRad, spreadRad);
            float randomY = UnityEngine.Random.Range(-spreadRad, spreadRad);

            var spreadRotation = Quaternion.Euler(
                randomY * Mathf.Rad2Deg,
                randomX * Mathf.Rad2Deg,
                z: 0
            );
            return spreadRotation * direction;
        }
    }
}