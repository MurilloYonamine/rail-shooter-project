using UnityEngine;
using UnityEngine.AI;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyChase : EnemyState
    {
        private EnemyController _enemy;
        private Transform _playerTarget;
        private float _chaseSpeed;

        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _chaseSpeed = _enemy.MovementSpeed * _enemy.ChaseMultiplicator;
            
            _enemy.Agent.isStopped = false;
            _enemy.Agent.speed = _chaseSpeed;
            
            _enemy.Animator.ResetAllAnimations();
            _enemy.Animator.SetIdle(false);
            
            _enemy.Animator.ForceRunningState();
            
            FindPlayer();
            
            Debug.Log("[EnemyChase] Entered chase state - Running animation force-activated");
        }

        public override void UpdateState()
        {
            if (_playerTarget == null)
            {
                FindPlayer();
                if (_playerTarget == null)
                {
                    _enemy.ChangeState(_enemy.PatrolState);
                    return;
                }
            }
            
            if (_enemy.Agent.isStopped)
            {
                _enemy.Agent.isStopped = false;
                _enemy.Animator.SetRunning(true);
            }
            
            float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _playerTarget.position);
            
            if (distanceToPlayer <= _enemy.AttackDistance)
            {
                _enemy.ChangeState(_enemy.AttackState);
                return;
            }
            
            ChasePlayer();
        }

        public override void ExitState()
        {
            if (_enemy.Agent.enabled)
            {
                _enemy.Agent.speed = _enemy.MovementSpeed;
            }
        }

        private void FindPlayer()
        {
            if (_enemy.Player != null)
            {
                _playerTarget = _enemy.Player;
                return;
            }
            
            Collider[] players = Physics.OverlapSphere(
                _enemy.transform.position, 
                50f, 
                _enemy.PlayerLayer
            );
            
            if (players.Length > 0)
            {
                _playerTarget = players[0].transform;
            }
        }

        private void ChasePlayer()
        {
            if (_playerTarget == null) return;
            
            Vector3 directionToPlayer = (_playerTarget.position - _enemy.transform.position).normalized;
            float randomAngle = Random.Range(-45f, 45f);
            Vector3 randomDirection = Quaternion.Euler(0, randomAngle, 0) * directionToPlayer;
            
            Vector3 targetPosition = _playerTarget.position + randomDirection * Random.Range(1f, 2f);
            
            _enemy.Agent.SetDestination(targetPosition);
            
            if (_enemy.Agent.speed != _chaseSpeed)
            {
                _enemy.Agent.speed = _chaseSpeed;
            }
        }
    }
}