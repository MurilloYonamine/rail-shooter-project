using RAIL_SHOOTER.AUDIO;
using RAIL_SHOOTER.PLAYER;
using UnityEngine;
using UnityEngine.AI;

namespace RAIL_SHOOTER.ENEMY
{
    [RequireComponent(typeof(Animator))]
    [RequireComponent(typeof(EnemyHealth))]
    [RequireComponent(typeof(NavMeshAgent))]
    public class EnemyController : MonoBehaviour
    {
        private EnemyAnimator _animator;
        private EnemyHealth _health;
        private NavMeshAgent _agent;
        [SerializeField] private float _movementSpeed = 3.5f;
        [SerializeField, Range(1f, 35f)] private float _patrolAreaRadius = 10f;
        [SerializeField, Range(0.5f, 15f)] private float _attackDistance = 2f;
        [SerializeField, Range(0.5f, 15f)] private float _chaseMultiplicator = 2f;
        [SerializeField] private LayerMask _playerLayer = ~3;

        private EnemyState _currentState;
        private EnemyIdle _idleState = new EnemyIdle();
        private EnemyPatrol _patrolState = new EnemyPatrol();
        private EnemyChase _chaseState = new EnemyChase();
        private EnemyScream _screamState = new EnemyScream();
        private EnemyDeath _deathState = new EnemyDeath();
        private EnemyAttack _attackState = new EnemyAttack();
        private EnemyHit _enemyHit = new EnemyHit();

        [SerializeField, Range(0.1f, 2f)] private float _hitCooldown = 0.5f;
        private float _lastHitTime = -1f;
        private bool _hasScreamed = false;
        
        [SerializeField] private AudioClip _screamClip;
        [SerializeField] private AudioClip[] _attackHitSounds;
        
        public AudioClip ScreamClip => _screamClip;
        public AudioClip[] AttackHitSounds => _attackHitSounds;
        
        public bool CanTakeHit => Time.time >= _lastHitTime + _hitCooldown;
        public bool HasScreamed => _hasScreamed;
        
        public void RegisterHit()
        {
            _lastHitTime = Time.time;
        }
        
        public void PlayRandomHitSound()
        {
            if (_attackHitSounds != null && _attackHitSounds.Length > 0 && AudioManager.Instance != null)
            {
                AudioClip randomHitSound = _attackHitSounds[Random.Range(0, _attackHitSounds.Length)];
                if (randomHitSound != null)
                {
                    AudioManager.Instance.PlaySFX(randomHitSound, volume: 1f);
                    Debug.Log("[EnemyController] Played attack hit sound");
                }
            }
        }
        
        public void MarkAsScreamed()
        {
            _hasScreamed = true;
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _health = GetComponent<EnemyHealth>();


            _agent.speed = _movementSpeed;
            Animator animatorComponent = GetComponent<Animator>();
            _animator = new EnemyAnimator(animatorComponent);
        }
        private void Start()
        {
            ChangeState(_patrolState);
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
        public NavMeshAgent Agent => _agent;
        public EnemyState IdleState => _idleState;
        public EnemyState PatrolState => _patrolState;
        public EnemyState ChaseState => _chaseState;
        public EnemyState ScreamState => _screamState;
        public EnemyState DeathState => _deathState;
        public EnemyState AttackState => _attackState;
        public EnemyState EnemyHit => _enemyHit;
        public float MovementSpeed => _movementSpeed;
        public float PatrolAreaRadius => _patrolAreaRadius;
        public float AttackDistance => _attackDistance;
        public LayerMask PlayerLayer => _playerLayer;
        public float ChaseMultiplicator => _chaseMultiplicator;

        #endregion

        #region Player Movement Control
        private PlayerController _playerController;
        private bool _playerMovementDisabled = false;
        
        public void DisablePlayerMovement()
        {
            if (_playerController == null)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    _playerController = player.GetComponent<PlayerController>();
                }
            }
            
            if (_playerController != null && !_playerMovementDisabled)
            {
                _playerController.PlayerMovement.SetMovementEnabled(false);
                _playerMovementDisabled = true;
            }
        }
        
        public void EnablePlayerMovement()
        {
            if (_playerController != null && _playerMovementDisabled)
            {
                _playerController.PlayerMovement.SetMovementEnabled(true);
                _playerMovementDisabled = false;
            }
        }
        #endregion

        private void OnDrawGizmosSelected()
        {
            if (_currentState == _patrolState && _patrolState != null)
            {
                ((EnemyPatrol)_patrolState).DrawPatrolAreaGizmo();
            }
            else
            {
                Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
                Gizmos.DrawSphere(transform.position, _patrolAreaRadius);

                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, _patrolAreaRadius);
            }
        }
    }
}
