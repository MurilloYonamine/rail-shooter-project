using System;
using System.Collections;
using RAIL_SHOOTER.PLAYER.INPUT;
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

        [Header("Booleans")]
        [SerializeField] private bool _isReloading = false;
        [SerializeField] private bool _isFiring = false;
        [SerializeField] private bool _isAiming = false;

        public bool IsReloading => _isReloading;
        public bool IsFiring => _isFiring;
        public bool IsAiming => _isAiming;

        public override void OnEnable()
        {
            InputReader.OnFirePressed += OnFirePressed;
            InputReader.OnFireReleased += OnFireReleased;
            InputReader.OnAimPressed += OnAimPressed;
            InputReader.OnAimReleased += OnAimReleased;
            InputReader.OnReloadPressed += OnReloadPressed;
        }

        public override void OnDisable()
        {
            InputReader.OnFirePressed -= OnFirePressed;
            InputReader.OnFireReleased -= OnFireReleased;
            InputReader.OnAimPressed -= OnAimPressed;
            InputReader.OnAimReleased -= OnAimReleased;
            InputReader.OnReloadPressed -= OnReloadPressed;
        }
        private void OnFirePressed()
        {
            if (!_isReloading && !_isFiring)
            {
                _player.StartCoroutine(PlayFireAnimation());
            }
        }
        private void OnReloadPressed()
        {
            if (!_isReloading && !_isFiring)
            {
                _player.StartCoroutine(PlayReloadAnimation());
            }
        }
        private void OnFireReleased()
        {
            // TODO: Implement in future
        }

        private void OnAimPressed() => SetAiming(true);
        private void OnAimReleased() => SetAiming(false);

        private void SetAiming(bool isAiming)
        {
            _isAiming = isAiming;

            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_AIMING, isAiming);
        }
        private IEnumerator PlayFireAnimation()
        {
            _isFiring = true;

            float animationDuration = _shootAnimationFrames / 60f;
            string armAnimatorParam = _isAiming ? ANIMATOR_PARAM_IS_FIRE_AIMING : ANIMATOR_PARAM_IS_FIRING;

            _armAnimator?.SetBool(armAnimatorParam, true);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_FIRING, true);

            yield return new WaitForSeconds(animationDuration);

            _armAnimator?.SetBool(armAnimatorParam, false);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_FIRING, false);

            _isFiring = false;
        }

        private IEnumerator PlayReloadAnimation()
        {
            _isReloading = true;

            float animationDuration = _reloadAnimationFrames / 60f;

            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, true);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, true);

            yield return new WaitForSeconds(animationDuration);

            _armAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, false);
            _gunAnimator?.SetBool(ANIMATOR_PARAM_IS_RELOADING, false);

            _isReloading = false;
        }
    }
}