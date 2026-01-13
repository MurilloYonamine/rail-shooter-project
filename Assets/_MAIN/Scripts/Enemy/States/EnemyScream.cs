using UnityEngine;
using System.Collections;
using RAIL_SHOOTER.AUDIO;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyScream : EnemyState
    {
        private EnemyController _enemy;
        private Transform _playerTarget;
        private bool _isScreamAnimationFinished = false;
        private float _screamTimer = 0f;
        private float _screamDuration = 2f;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Agent.isStopped = true;
            _enemy.Animator.ResetAllAnimations();
            
            _enemy.DisablePlayerMovement();
            _enemy.MarkAsScreamed();
            
            FindPlayer();
            
            if (_playerTarget != null)
            {
                LookAtPlayerImmediate();
            }
            
            _isScreamAnimationFinished = false;
            _screamTimer = 0f;
            
            _enemy.Animator.PlayScreamAnimation();
            AudioManager.Instance.PlaySFX(_enemy.ScreamClip, volume: 0.9f, pitch: 0.5f);
            
            Debug.Log("[EnemyScream] Entered scream state");
        }

        public override void UpdateState()
        {
            _screamTimer += Time.deltaTime;
            
            if (_playerTarget != null)
            {
                LookAtPlayer();
            }
            else
            {
                FindPlayer();
            }
            
            if (_screamTimer >= _screamDuration)
            {
                _enemy.ChangeState(_enemy.ChaseState);
            }
        }

        public override void ExitState()
        {
            _enemy.Animator.ResetAllAnimations();
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
                100f, 
                _enemy.PlayerLayer
            );
            if (players.Length > 0)
            {
                _playerTarget = players[0].transform;
                return;
            }
        }
        
        private void LookAtPlayerImmediate()
        {
            if (_playerTarget == null) return;
            
            Vector3 directionToPlayer = (_playerTarget.position - _enemy.transform.position).normalized;
            directionToPlayer.y = 0;
            
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                _enemy.transform.rotation = lookRotation; 
            }
        }
        
        private void LookAtPlayer()
        {
            if (_playerTarget == null) return;
            
            Vector3 directionToPlayer = (_playerTarget.position - _enemy.transform.position).normalized;
            directionToPlayer.y = 0;
            
            if (directionToPlayer != Vector3.zero)
            {
                Quaternion lookRotation = Quaternion.LookRotation(directionToPlayer);
                _enemy.transform.rotation = Quaternion.Slerp(_enemy.transform.rotation, lookRotation, Time.deltaTime * 10f);
            }
        }
    }
}