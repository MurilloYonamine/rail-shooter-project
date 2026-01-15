using UnityEngine;
using UnityEngine.AI;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyChase : EnemyState
    {
        private Transform _playerTarget;
        private float _chaseSpeed;

        public override void EnterState(EnemyController enemy)
        {
            base.EnterState(enemy);
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {

        }

        private void FindPlayer()
        {

        }

        private void ChasePlayer()
        {
            if (!_playerTarget) return;
            
            var directionToPlayer = (_playerTarget.position - _enemy.transform.position).normalized;
            var randomAngle = Random.Range(-45f, 45f);
            var randomDirection = Quaternion.Euler(0, randomAngle, 0) * directionToPlayer;
            
            var targetPosition = _playerTarget.position + randomDirection * Random.Range(1f, 2f);
            
            _enemy.Agent.SetDestination(targetPosition);

            if (Mathf.Approximately(_enemy.Agent.speed, _chaseSpeed)) return;
            
            _enemy.Agent.speed = _chaseSpeed;
        }
    }
}