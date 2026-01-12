using System.Collections;
using RAIL_SHOOTER.ENEMY;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerBullet : MonoBehaviour
    {
        [field: SerializeField, Range(1, 10)] private float _lifetime = 2f;
        [field: SerializeField, Range(1, 5)] private int _damage = 1;

        private IEnumerator LifetimeRoutine()
        {
            yield return new WaitForSeconds(_lifetime);
            BulletPool.Instance.ReturnBullet(this);
        }
        
        private void OnEnable()
        {
            StartCoroutine(LifetimeRoutine());
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent<EnemyHealth>(out EnemyHealth enemy))
            {
                BulletPool.Instance.ReturnBullet(this);
                enemy.TakeDamage(_damage);
            }
        }
    }
}