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
        
        [Header("Accuracy")]
        [SerializeField] private float _hipFireSpread = 5f; // Graus de dispersão quando não mira
        [SerializeField] private float _aimSpread = 1f; // Graus de dispersão quando mira

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
