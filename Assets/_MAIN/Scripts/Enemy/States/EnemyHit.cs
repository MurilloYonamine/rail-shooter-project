using System.Collections;
using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyHit : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            
            _enemy.Animator.SetIdle(false);
            _enemy.Animator.SetWalking(false);
            _enemy.Animator.SetRunning(false);
            
            _enemy.Animator.PlayHitAnimation();
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
            if (_enemy.Animator.IsHitAnimationFinished())
            {
                _enemy.ChangeState(_enemy.IdleState);
            }
        }
    }
}