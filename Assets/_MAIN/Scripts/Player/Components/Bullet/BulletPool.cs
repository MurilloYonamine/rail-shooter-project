using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class BulletPool : MonoBehaviour
    {
        public static BulletPool Instance { get; private set; }

        [SerializeField] private Transform _parentTransform;
        [SerializeField] private PlayerBullet _bulletPrefab;
        [SerializeField] private int _initialCapacity = 10;

        private Stack<PlayerBullet> _availableBullets;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
        }
        private void Start()
        {
            _availableBullets = new Stack<PlayerBullet>(_initialCapacity);
            for (int i = 0; i < _initialCapacity; i++)
            {
                PlayerBullet bullet = Instantiate(_bulletPrefab, _parentTransform);
                bullet.gameObject.SetActive(false);
                _availableBullets.Push(bullet);
            }
        }
        public PlayerBullet GetBullet()
        {
            if (_availableBullets.Count > 0)
            {
                PlayerBullet bullet = _availableBullets.Pop();
                bullet.gameObject.SetActive(true);
                return bullet;
            }
            return null;
        }
        public void ReturnBullet(PlayerBullet bullet)
        {
            bullet.gameObject.SetActive(false);
            _availableBullets.Push(bullet);
            bullet.transform.SetParent(_parentTransform);
        }
    }
}