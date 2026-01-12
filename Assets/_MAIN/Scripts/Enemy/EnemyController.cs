using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyHealth))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Components")]
        private EnemyAnimator _animator;
        private EnemyHealth _health;

        [Header("Enemy States")]
        private EnemyState _currentState;
        private EnemyIdle _idleState = new EnemyIdle();
        private EnemyWalking _walkingState = new EnemyWalking();
        private EnemyRunning _runningState = new EnemyRunning();
        private EnemyScream _screamState = new EnemyScream();
        private EnemyDeath _deathState = new EnemyDeath();
        private EnemyAttack _attackState = new EnemyAttack();
        private EnemyHit _enemyHit = new EnemyHit();

        private void Awake()
        {
            _health = GetComponent<EnemyHealth>();

            Animator animatorComponent = GetComponent<Animator>();
            _animator = new EnemyAnimator(animatorComponent);
        }
        private void Start()
        {
            ChangeState(_idleState);
        }
        private void Update()
        {
            _currentState?.UpdateState();
        }

        public void ChangeState(EnemyState newState)
        {
            _currentState?.ExitState();
            _currentState = newState;
            _currentState.EnterState(this);
        }

        #region Properties
        public EnemyAnimator Animator => _animator;
        public EnemyState IdleState => _idleState;
        public EnemyState WalkingState => _walkingState;
        public EnemyState RunningState => _runningState;
        public EnemyState ScreamState => _screamState;
        public EnemyState DeathState => _deathState;
        public EnemyState AttackState => _attackState;
        public EnemyState EnemyHit => _enemyHit;
        #endregion
    }
}
