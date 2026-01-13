using System.Collections;
using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyHit : EnemyState
    {
        private EnemyController _enemy;
        private float _hitTimer = 0f;
        private float _hitAnimationDuration = 0f;
        private float _maxHitTimeout = 1f;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _hitTimer = 0f;
            
            _enemy.Agent.isStopped = true;
            _enemy.Agent.velocity = Vector3.zero;
            
            _enemy.Animator.SetIdle(false);
            _enemy.Animator.SetWalking(false);
            _enemy.Animator.SetRunning(false);
            
            _enemy.Animator.PlayHitAnimation();
            
            _hitAnimationDuration = 0.5f;
        }

        public override void ExitState()
        {
            if (_enemy.Agent.enabled)
            {
                _enemy.Agent.isStopped = false;
            }
        }

        public override void UpdateState()
        {
            _hitTimer += Time.deltaTime;
            
            bool timerReached = _hitTimer >= _hitAnimationDuration;
            bool timeoutReached = _hitTimer >= _maxHitTimeout;
            
            if (timerReached || timeoutReached)
            {
                if (_enemy.Player != null)
                {
                    float distanceToPlayer = Vector3.Distance(_enemy.transform.position, _enemy.Player.position);
                    
                    if (distanceToPlayer <= _enemy.AttackDistance + 1f)
                    {
                        _enemy.ChangeState(_enemy.AttackState);
                    }
                    else if (!_enemy.HasScreamed)
                    {
                        _enemy.ChangeState(_enemy.ScreamState);
                    }
                    else
                    {
                        _enemy.ChangeState(_enemy.ChaseState);
                    }
                }
                else
                {
                    _enemy.ChangeState(_enemy.PatrolState);
                }
            }
        }
    }
}