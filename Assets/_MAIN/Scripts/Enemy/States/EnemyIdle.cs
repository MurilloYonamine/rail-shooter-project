using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyIdle : EnemyState
    {
        private EnemyController _enemy;
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.SetIdle(true);
            Debug.Log("[EnemyIdle] Entered idle state");
        }

        public override void ExitState()
        {
            _enemy.Animator.SetIdle(false);
        }

        public override void UpdateState()
        {
        }
    }
}