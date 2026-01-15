using UnityEngine;
using System.Collections;
using RAIL_SHOOTER.AUDIO;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyScream : EnemyState
    {
        private Transform _playerTarget;

        public override void EnterState(EnemyController enemy)
        {
            base.EnterState(enemy);
        }

        public override void UpdateState()
        {
            throw new System.NotImplementedException();
        }

        public override void ExitState()
        {
            throw new System.NotImplementedException();
        }
        private void LookAtPlayer()
        {
            if (_playerTarget == null) return;
            
            var directionToPlayer = (_playerTarget.position - _enemy.transform.position).normalized;
            directionToPlayer.y = 0;

            const float epsilon = 0.0001f;
            if (directionToPlayer.sqrMagnitude < epsilon) return;
            
            var lookRotation = Quaternion.LookRotation(directionToPlayer);
            _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }
}