using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyAnimator
    {
        [Header("Animator Parameters")]
        private const string ANIMATOR_PARAM_IS_IDLE = "isIdle";
        private const string ANIMATOR_PARAM_IS_WALKING = "isWalking";
        private const string ANIMATOR_PARAM_IS_RUNNING = "isRunning";
        private const string ANIMATOR_PARAM_ATTACK_TRIGGER = "SimpleAttack";
        private const string ANIMATOR_PARAM_LUMBERJACK_ATTACK_TRIGGER = "LumberjackAttack";
        private const string ANIMATOR_PARAM_DEATH_TRIGGER = "Death";
        private const string ANIMATOR_PARAM_SCREAM_TRIGGER = "Scream";
        private const string ANIMATOR_PARAM_HIT_TRIGGER = "Hit";

        private Animator _animator;

        public EnemyAnimator(Animator animator)
        {
            _animator = animator;
        }
        public void SetIdle(bool isIdle)
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_IDLE, isIdle);
        }
        public void SetWalking(bool isWalking)
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_WALKING, isWalking);
        }
        public void SetRunning(bool isRunning)
        {
            _animator.SetBool(ANIMATOR_PARAM_IS_RUNNING, isRunning);
        }
        public void PlayAttackAnimation()
        {
            _animator.SetTrigger(ANIMATOR_PARAM_ATTACK_TRIGGER);
        }
        public void PlayLumberjackAttackAnimation()
        {
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
            _animator.SetTrigger(ANIMATOR_PARAM_HIT_TRIGGER);
        }
    }
}