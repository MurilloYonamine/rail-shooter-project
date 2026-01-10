using System;
using System.Collections;
using RAIL_SHOOTER.PLAYER.INPUT;
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

        [SerializeField] private GameObject _bulletPrefab;
        [SerializeField] private Transform _shootPoint;
        [SerializeField] private float _shootForce = 700f;
        [SerializeField] private float _shootCooldown = 0.5f;

        private float _lastShootTime;
        public override void OnEnable()
        {
            InputReader.OnAttackPressed += OnAttackPressed;
            InputReader.OnAttackReleased += OnAttackReleased;
        }

        public override void OnDisable()
        {
            InputReader.OnAttackPressed -= OnAttackPressed;
            InputReader.OnAttackReleased -= OnAttackReleased;
        }
        private void OnAttackPressed()
        {
            Shoot();
        }
        private void OnAttackReleased()
        {
        }

        private void Shoot()
        {
            if (Time.time < _lastShootTime + _shootCooldown)
                return;

            Vector3 aimWorldPosition = _player.PlayerAim.GetAimWorldPosition(100f);

            Vector3 shootDirection = (aimWorldPosition - _shootPoint.position).normalized;

            Debug.DrawRay(_shootPoint.position, shootDirection * 10f, Color.red, 1f);

            Quaternion bulletRotation = Quaternion.LookRotation(shootDirection);

            // GameObject bullet = GameObject.Instantiate(
            //     _bulletPrefab,
            //     _shootPoint.position,
            //     bulletRotation
            // );
            PlayerBullet bullet = BulletPool.Instance.GetBullet();
            bullet.transform.SetPositionAndRotation(
                _shootPoint.position,
                bulletRotation
            );
            bullet.transform.parent = null;

            Rigidbody rigidBody = bullet.GetComponent<Rigidbody>();

            rigidBody.velocity = Vector3.zero;
            rigidBody.angularVelocity = Vector3.zero;

            rigidBody.AddForce(shootDirection * _shootForce, ForceMode.Impulse);

            _lastShootTime = Time.time;
        }

    }
}
