using System;
using System.Collections;
using RAIL_SHOOTER.PLAYER.INPUT;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerShoot : PlayerComponent
    {
        public enum FireMode { SemiAuto, FullAuto, Burst }

        [Header("Weapon")]
        [SerializeField] private FireMode _fireMode;
        [SerializeField] private float _fireRate = 4f;
        [SerializeField] private int _burstCount = 3;

        [Header("Shoot")]
        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootForce = 700f;

        private Coroutine _shootRoutine;

        public override void OnEnable()
        {
            InputReader.OnAttackPressed += OnAttackPressed;
            InputReader.OnAttackReleased += OnAttackReleased;
        }

        public override void OnDisable()
        {
            InputReader.OnAttackPressed -= OnAttackPressed;
            InputReader.OnAttackReleased -= OnAttackReleased;

            StopShooting();
        }

        private void OnAttackPressed()
        {
            switch (_fireMode)
            {
                case FireMode.SemiAuto:
                    ShootOnce();
                    break;

                case FireMode.FullAuto:
                    StartShootingLoop();
                    break;

                case FireMode.Burst:
                    _player.StartCoroutine(BurstRoutine());
                    break;
            }
        }

        private void OnAttackReleased()
        {
            if (_fireMode == FireMode.FullAuto)
            {
                StopShooting();
            }
        }

        private void ShootOnce()
        {
            Shoot();
        }

        private void StartShootingLoop()
        {
            if (_shootRoutine != null)
            {
                return;
            }

            _shootRoutine = _player.StartCoroutine(AutoFireRoutine());
        }

        private void StopShooting()
        {
            if (_shootRoutine == null)
            {
                return;
            }

            _player.StopCoroutine(_shootRoutine);
            _shootRoutine = null;
        }

        private IEnumerator AutoFireRoutine()
        {
            float interval = 1f / _fireRate;

            while (true)
            {
                Shoot();
                yield return new WaitForSeconds(interval);
            }
        }

        private IEnumerator BurstRoutine()
        {
            float interval = 1f / _fireRate;

            for (int i = 0; i < _burstCount; i++)
            {
                Shoot();
                yield return new WaitForSeconds(interval);
            }
        }

        private void Shoot()
        {
            Vector3 aimPosition = _player.PlayerAim.GetAimWorldPosition(100f);
            Vector3 bulletDirection = (aimPosition - _shootPoint.position).normalized;

            Debug.DrawRay(_shootPoint.position, bulletDirection * 10f, Color.red, 1f);

            GameObject bullet = GameObject.Instantiate(
                _bulletPrefab,
                _shootPoint.position,
                Quaternion.LookRotation(bulletDirection)
            );

            Rigidbody rb = bullet.GetComponent<Rigidbody>();
            rb.AddForce(bulletDirection * _shootForce, ForceMode.Impulse);
        }
    }
}
