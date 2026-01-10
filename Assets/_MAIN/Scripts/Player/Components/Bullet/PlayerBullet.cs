using System.Collections;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerBullet : MonoBehaviour
    {
        [field: SerializeField, Range(1, 10)] private float _lifetime = 2f;

        private IEnumerator LifetimeRoutine()
        {
            yield return new WaitForSeconds(_lifetime);
            BulletPool.Instance.ReturnBullet(this);
        }
        private void OnEnable()
        {
            StartCoroutine(LifetimeRoutine());
        }
    }
}