using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    [RequireComponent(typeof(Animator))]
    public class EnemyController : MonoBehaviour
    {
        [Header("Components")]
        [SerializeField] private EnemyHealth _health;
        private EnemyAnimator _animator;

        [Header("Enemy States")]
        private EnemyState _currentState;
        private EnemyIdle _idleState = new EnemyIdle();
        private EnemyWalking _walkingState = new EnemyWalking();
        private EnemyRunning _runningState = new EnemyRunning();
        private EnemyScream _screamState = new EnemyScream();
        private EnemyDeath _deathState = new EnemyDeath();
        private EnemyAttack _attackState = new EnemyAttack();

        [Header("Testing - Old Input System")]
        [SerializeField] private bool enableInputTesting = true;

        private void Awake()
        {
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

            if (enableInputTesting)
            {
                TestStates();
            }
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
        #endregion

        private void TestStates()
        {
            if (Input.GetKeyDown(KeyCode.Keypad1))
            {
                Debug.Log("Switching to Idle State");
                ChangeState(_idleState);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad2))
            {
                Debug.Log("Switching to Walking State");
                ChangeState(_walkingState);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                Debug.Log("Switching to Running State");
                ChangeState(_runningState);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad4))
            {
                Debug.Log("Switching to Scream State");
                ChangeState(_screamState);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad5))
            {
                Debug.Log("Switching to Death State");
                ChangeState(_deathState);
            }
            else if (Input.GetKeyDown(KeyCode.Keypad6))
            {
                Debug.Log("Switching to Attack State");
                ChangeState(_attackState);
            }
        }
    }
}
