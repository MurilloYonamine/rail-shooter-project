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
        [SerializeField] private float _shootCooldown = 0.5f;
        [SerializeField] private float _range = 100f;

        private float _lastShootTime;

        public override void OnEnable()
        {
            _player.OnPlayerFirePressed += OnFirePressed;
            _player.OnPlayerFireReleased += OnFireReleased;
        }

        public override void OnDisable()
        {
            _player.OnPlayerFirePressed -= OnFirePressed;
            _player.OnPlayerFireReleased -= OnFireReleased;
        }

        private void OnFirePressed()
        {
            Shoot();
        }

        private void OnFireReleased()
        {
        }

        private void Shoot()
        {
            if (Time.time < _lastShootTime + _shootCooldown)
            {
                return;
            }

            if (!_player.PlayerAim.TryGetAimPoint(out Vector3 aimPoint, _range))
            {
                return;
            }

            Vector3 shootDirection = (aimPoint - _shootPoint.position).normalized;

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

            _lastShootTime = Time.time;
        }
    }
}
