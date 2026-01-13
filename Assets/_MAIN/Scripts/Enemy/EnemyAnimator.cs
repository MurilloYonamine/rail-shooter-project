using System.Collections;
using UnityEngine;
using RAIL_SHOOTER.AUDIO;
using RAIL_SHOOTER.MANAGERS;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyAnimator
    {
        private const string ANIMATOR_PARAM_IS_IDLE = "isIdle";
        private const string ANIMATOR_PARAM_IS_WALKING = "isWalking";
        private const string ANIMATOR_PARAM_IS_RUNNING = "isRunning";
        private const string ANIMATOR_PARAM_ATTACK_TRIGGER = "SimpleAttack";
        private const string ANIMATOR_PARAM_LUMBERJACK_ATTACK_TRIGGER = "LumberjackAttack";
        private const string ANIMATOR_PARAM_DEATH_TRIGGER = "Death";
        private const string ANIMATOR_PARAM_SCREAM_TRIGGER = "Scream";
        private const string ANIMATOR_PARAM_HIT_TRIGGER = "Hit";

        private Animator _animator;
        private MonoBehaviour _enemyMonoBehaviour;

        private float _walkingFootstepInterval = 0.6f;
        private float _runningFootstepInterval = 0.4f;

        private Coroutine _footstepCoroutine;
        private bool _isWalking = false;
        private bool _isRunning = false;

        public EnemyAnimator(Animator animator)
        {
            _animator = animator;
            _enemyMonoBehaviour = animator.GetComponent<MonoBehaviour>();
        }
        public void SetIdle(bool isIdle)
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, isIdle);
        }
        public void SetWalking(bool isWalking)
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_WALKING, isWalking);
            _isWalking = isWalking;

            if (isWalking)
            {
                _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            }

            if (isWalking && !_isRunning)
            {
                StartFootsteps(false);
            }
            else if (!isWalking && !_isRunning)
            {
                StopFootsteps();
            }
        }
        public void SetRunning(bool isRunning)
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_RUNNING, isRunning);
            _isRunning = isRunning;

            if (isRunning)
            {
                _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            }

            if (isRunning)
            {
                StartFootsteps(true);
            }
            else if (!_isWalking)
            {
                StopFootsteps();
            }
        }
        public void PlayAttackAnimation()
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            _animator.SetTrigger(ANIMATOR_PARAM_ATTACK_TRIGGER);
        }
        public void PlayLumberjackAttackAnimation()
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            _animator.SetTrigger(ANIMATOR_PARAM_LUMBERJACK_ATTACK_TRIGGER);
        }
        public void PlayDeathAnimation()
        {
            _animator.SetTrigger(ANIMATOR_PARAM_DEATH_TRIGGER);
        }
        public void PlayScreamAnimation()
        {
            _animator.SetTrigger(ANIMATOR_PARAM_SCREAM_TRIGGER);
        }
        public void PlayHitAnimation()
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            _animator.SetTrigger(ANIMATOR_PARAM_HIT_TRIGGER);
        }

        public void ForceRunningState()
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            _animator.SetBool(ANIMATOR_PARAM_IS_WALKING, false);
            _animator.SetBool(ANIMATOR_PARAM_IS_RUNNING, false);

            _animator.SetBool(ANIMATOR_PARAM_IS_RUNNING, true);
            _isRunning = true;
            _isWalking = false;

            StartFootsteps(true);
        }

        public void ResetAllAnimations()
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            _animator.SetBool(ANIMATOR_PARAM_IS_WALKING, false);
            _animator.SetBool(ANIMATOR_PARAM_IS_RUNNING, false);

            _animator.ResetTrigger(ANIMATOR_PARAM_ATTACK_TRIGGER);
            _animator.ResetTrigger(ANIMATOR_PARAM_LUMBERJACK_ATTACK_TRIGGER);
            _animator.ResetTrigger(ANIMATOR_PARAM_DEATH_TRIGGER);
            _animator.ResetTrigger(ANIMATOR_PARAM_SCREAM_TRIGGER);
            _animator.ResetTrigger(ANIMATOR_PARAM_HIT_TRIGGER);

            _isWalking = false;
            _isRunning = false;
            StopFootsteps();
        }

        public void ForceCleanState()
        {
            ResetAllAnimations();
        }

        public void SetIdleState()
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, false);
            _animator.SetBool(ANIMATOR_PARAM_IS_WALKING, false);
            _animator.SetBool(ANIMATOR_PARAM_IS_RUNNING, false);
            _isWalking = false;
            _isRunning = false;
            StopFootsteps();

            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, true);
        }

        public bool IsHitAnimationFinished()
        {
            AnimatorStateInfo stateInfo = _animator.GetCurrentAnimatorStateInfo(0);

            bool isInHitState = stateInfo.IsName("Hit") || stateInfo.IsName("Reaction Hit");

            if (isInHitState)
            {
                bool finished = stateInfo.normalizedTime >= 0.9f;
                return finished;
            }

            return true;
        }

        #region Footstep System
        private void StartFootsteps(bool isRunning)
        {
            if (_footstepCoroutine != null)
            {
                _enemyMonoBehaviour.StopCoroutine(_footstepCoroutine);
            }
            _footstepCoroutine = _enemyMonoBehaviour.StartCoroutine(PlayFootstepsLoop(isRunning));
        }

        private void StopFootsteps()
        {
            if (_footstepCoroutine != null)
            {
                _enemyMonoBehaviour.StopCoroutine(_footstepCoroutine);
                _footstepCoroutine = null;
            }
        }

        private IEnumerator PlayFootstepsLoop(bool isRunning)
        {
            float interval = isRunning ? _runningFootstepInterval : _walkingFootstepInterval;
            while ((isRunning && _isRunning) || (!isRunning && _isWalking && !_isRunning))
            {
                bool canPlayFootstep = false;
                EnemyController enemyController = _animator.GetComponent<EnemyController>();
                if (enemyController != null && enemyController.PlayerController != null)
                {
                    float patrolRadius = enemyController.PatrolAreaRadius;
                    float maxDistance = patrolRadius * 1.75f;
                    float playerDistance = Vector3.Distance(enemyController.transform.position, enemyController.PlayerController.transform.position);
                    if (playerDistance <= maxDistance)
                        canPlayFootstep = true;
                }
                if (canPlayFootstep && GameManager.Instance != null)
                {
                    AudioClip currentFootstep = GameManager.Instance.GetRandomFootstepSound();
                    if (currentFootstep != null && AudioManager.Instance != null)
                    {
                        float volume = isRunning ? 0.25f : 0.15f;
                        float pitch = isRunning ? Random.Range(0.9f, 1.1f) : Random.Range(0.8f, 1.0f);
                        float blend = 1f;
                        AudioManager.Instance.PlaySFX(currentFootstep, volume: volume, pitch: pitch, blend: blend);
                    }
                }
                yield return new WaitForSeconds(interval);
            }
        }
        #endregion
    }
}