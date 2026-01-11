using System;
using System.Collections;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerAnimator : PlayerComponent
    {
        [Header("Animators")]
        [SerializeField] private Animator _armAnimator;
        [SerializeField] private Animator _gunAnimator;

        [Header("Animation Durations (in frames / 60fps)")]
        [SerializeField] private float _shootAnimationFrames = 14f;
        [SerializeField] private float _reloadAnimationFrames = 72f;

        [Header("Animator Parameters")]
        private const string ANIMATOR_PARAM_IS_FIRING = "isFiring";
        private const string ANIMATOR_PARAM_IS_RELOADING = "isReloading";
        private const string ANIMATOR_PARAM_IS_AIMING = "isAiming";
        private const string ANIMATOR_PARAM_IS_FIRE_AIMING = "isFireAiming";
        private const string ANIMATOR_PARAM_IS_WALKING = "isWalking";
        private const string ANIMATOR_PARAM_WALK_AIMING = "isWalkAiming";

        public override void OnEnable()
        {
            _player.OnPlayerFirePressed += OnFirePressed;
            _player.OnPlayerFireReleased += OnFireReleased;
            _player.OnPlayerAimPressed += OnAimPressed;
            _player.OnPlayerAimReleased += OnAimReleased;
            _player.OnPlayerReloadPressed += OnReloadPressed;
        }

        public override void OnDisable()
        {
            _player.OnPlayerFirePressed -= OnFirePressed;
            _player.OnPlayerFireReleased -= OnFireReleased;
            _player.OnPlayerAimPressed -= OnAimPressed;
            _player.OnPlayerAimReleased -= OnAimReleased;
            _player.OnPlayerReloadPressed -= OnReloadPressed;
        }

        private void OnFirePressed()
        {
            _player.StartCoroutine(PlayFireAnimation());
        }

        private void OnReloadPressed()
        {
            _player.StartCoroutine(PlayReloadAnimation());
        }

        private void OnFireReleased()
        {
            // TODO: Implement in future
        }

        private void OnAimPressed() => SetAiming(true);
        private void OnAimReleased() => SetAiming(false);

        private void SetAiming(bool isAiming)
        {
            _player.SetAiming(isAiming);
            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_AIMING, isAiming);
        }

        private IEnumerator PlayFireAnimation()
        {
            _player.SetFiring(true);

            float animationDuration = _shootAnimationFrames / 60f;
            string armAnimatorParam = _player.IsAiming ? ANIMATOR_PARAM_IS_FIRE_AIMING : ANIMATOR_PARAM_IS_FIRING;

            _armAnimator?.SetBool(armAnimatorParam, true);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_FIRING, true);

            yield return new WaitForSeconds(animationDuration);

            _armAnimator?.SetBool(armAnimatorParam, false);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_FIRING, false);

            _player.SetFiring(false);
        }

        private IEnumerator PlayReloadAnimation()
        {
            _player.SetReloading(true);

            float animationDuration = _reloadAnimationFrames / 60f;

            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, true);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, true);

            yield return new WaitForSeconds(animationDuration);

            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, false);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, false);

            _player.SetReloading(false);
        }
        public void SetWalking(bool isWalking)
        {
            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_WALKING, isWalking);
            if (_player.IsAiming)
            {
                _armAnimator?.SetBool(ANIMATOR_PARAM_WALK_AIMING, isWalking);
            }
        } 
    }
}