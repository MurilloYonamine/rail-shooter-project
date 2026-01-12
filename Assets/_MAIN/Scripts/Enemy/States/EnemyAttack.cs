using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyAttack : EnemyState
    {
        private EnemyController _enemy;
        private float _attackTimer = 0f;
        private float _attackDuration = 1.5f;
        private float _attackCooldown = 1.0f;
        private bool _hasAttacked = false;
        private bool _isInCooldown = false;
        private bool _hasPlayedHitSound = false;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Agent.isStopped = true;
            
            _enemy.Agent.velocity = Vector3.zero;
            _enemy.Agent.ResetPath();
            
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                Vector3 directionToPlayer = (player.transform.position - _enemy.transform.position).normalized;
                
                float randomAngle = Random.Range(-45f, 45f);
                Quaternion randomRotation = Quaternion.Euler(0, randomAngle, 0);
                Vector3 finalDirection = randomRotation * directionToPlayer;
                
                _enemy.transform.rotation = Quaternion.LookRotation(finalDirection);
            }
            else
            {
                float randomY = Random.Range(0f, 360f);
                _enemy.transform.rotation = Quaternion.Euler(0, randomY, 0);
            }
            
            _enemy.Animator.ResetAllAnimations();
            
            _attackTimer = 0f;
            _hasAttacked = false;
            _isInCooldown = false;
            _hasPlayedHitSound = false;
            
            if (Random.Range(0f, 1f) > 0.5f)
            {
                _enemy.Animator.PlayAttackAnimation();
            }
            else
            {
                _enemy.Animator.PlayLumberjackAttackAnimation();
            }
            
            Debug.Log("[EnemyAttack] Entered attack state");
        }

        public override void UpdateState()
        {
            _attackTimer += Time.deltaTime;
            
            if (_attackTimer >= _attackDuration * 0.8f && !_hasPlayedHitSound && !_isInCooldown)
            {
                _enemy.PlayRandomHitSound();
                _hasPlayedHitSound = true;
            }
            
            if (_attackTimer >= _attackDuration && !_isInCooldown)
            {
                _isInCooldown = true;
                _attackTimer = 0f;
            }
            else if (_isInCooldown && _attackTimer >= _attackCooldown)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    float distance = Vector3.Distance(_enemy.transform.position, player.transform.position);
                    if (distance <= 3f)
                    {
                        _attackTimer = 0f;
                        _isInCooldown = false;
                        _hasPlayedHitSound = false;
                        
                        _enemy.Animator.ResetAllAnimations();
                        
                        if (Random.Range(0f, 1f) > 0.5f)
                        {
                            _enemy.Animator.PlayAttackAnimation();
                        }
                        else
                        {
                            _enemy.Animator.PlayLumberjackAttackAnimation();
                        }
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

        public override void ExitState()
        {
            _enemy.Agent.isStopped = false;
        }
    }
}