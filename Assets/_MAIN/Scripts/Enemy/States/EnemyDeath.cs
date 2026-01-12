using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyDeath : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.PlayDeathAnimation();
            Debug.Log("[EnemyDeath] Entered death state");
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
        }
    }
}