using UnityEngine;
using UnityEngine.AI;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyPatrol : EnemyState
    {
        private Vector3 _currentDestination;
        private Vector3 _patrolCenter;

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

            if (playersInArea.Length <= 0) return;
            
            Gizmos.color = Color.red;
            Gizmos.DrawLine(_enemy.transform.position, playersInArea[0].transform.position);
        }
    }
}