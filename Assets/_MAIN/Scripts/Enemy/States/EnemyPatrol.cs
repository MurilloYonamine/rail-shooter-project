using UnityEngine;
using UnityEngine.AI;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyPatrol : EnemyState
    {
        private EnemyController _enemy;
        private bool _isWaiting;
        private float _waitTimer;
        private Vector3 _currentDestination;
        private Vector3 _patrolCenter;

        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            
            _enemy.Animator.ResetAllAnimations();
            _enemy.Animator.SetIdle(false);
            _enemy.Animator.SetWalking(true);
            _enemy.Agent.isStopped = false;
            
            _isWaiting = false;
            _waitTimer = 0f;
            
            FindRandomDestination();
            
            Debug.Log("[EnemyPatrol] Entered patrol state");
        }

        public override void UpdateState()
        {
            CheckForPlayerInPatrolArea();
            
            if (_isWaiting)
            {
                _waitTimer += Time.deltaTime;
                if (_waitTimer >= 2f)
                {
                    _isWaiting = false;
                    _waitTimer = 0f;
                    FindRandomDestination();
                }
            }
            else
            {
                if (!_enemy.Agent.pathPending)
                {
                    if (_enemy.Agent.remainingDistance < 1f || _enemy.Agent.pathStatus == NavMeshPathStatus.PathInvalid)
                    {
                        _isWaiting = true;
                        _enemy.Agent.isStopped = true;
                        _enemy.Animator.SetWalking(false);
                    }
                }
            }
        }

        private void CheckForPlayerInPatrolArea()
        {
            _patrolCenter = _enemy.transform.position;
            
            Collider[] playersInArea = Physics.OverlapSphere(_patrolCenter, _enemy.PatrolAreaRadius, _enemy.PlayerLayer);
            
            if (playersInArea.Length > 0)
            {
                _enemy.ChangeState(_enemy.ScreamState);
            }
        }

        public override void ExitState()
        {
            if (_enemy.Agent.enabled)
            {
                _enemy.Agent.isStopped = true;
                _enemy.Animator.SetWalking(false);
            }
        }

        private void FindRandomDestination()
        {
            _patrolCenter = _enemy.transform.position;
            
            int attempts = 0;
            int maxAttempts = 10;
            
            while (attempts < maxAttempts)
            {
                Vector3 randomDirection = Random.insideUnitSphere * _enemy.PatrolAreaRadius;
                randomDirection += _patrolCenter;
                randomDirection.y = _patrolCenter.y; 

                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomDirection, out hit, _enemy.PatrolAreaRadius, NavMesh.AllAreas))
                {
                    NavMeshPath path = new NavMeshPath();
                    if (_enemy.Agent.CalculatePath(hit.position, path) && path.status == NavMeshPathStatus.PathComplete)
                    {
                        _currentDestination = hit.position;
                        _enemy.Agent.isStopped = false;
                        _enemy.Agent.SetDestination(_currentDestination);
                        _enemy.Animator.SetWalking(true);
                        return; 
                    }
                }
                
                attempts++;
            }
            
            _isWaiting = true;
            _waitTimer = 0f;
        }

        public void DrawPatrolAreaGizmo()
        {
            if (_enemy == null) return;
            
            Gizmos.color = new Color(0f, 1f, 0f, 0.3f);
            Gizmos.DrawSphere(_patrolCenter, _enemy.PatrolAreaRadius);
            
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(_patrolCenter, _enemy.PatrolAreaRadius);
            
            if (_currentDestination != Vector3.zero)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_currentDestination, 0.5f);
                
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(_enemy.transform.position, _currentDestination);
            }
            
            Collider[] playersInArea = Physics.OverlapSphere(
                _patrolCenter, 
                _enemy.PatrolAreaRadius, 
                _enemy.PlayerLayer
            );

            if (playersInArea.Length > 0)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawLine(_enemy.transform.position, playersInArea[0].transform.position);
            }
        }
    }
}