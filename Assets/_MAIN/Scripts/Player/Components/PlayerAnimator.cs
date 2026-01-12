using System;
using System.Collections;
using UnityEngine;
using RAIL_SHOOTER.AUDIO;

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

        [Header("Footstep Audio")]
        [SerializeField] private AudioClip[] _footstepSounds;
        [SerializeField] private float _footstepInterval = 0.5f;

        private bool _isWalking = false;
        private Coroutine _footstepCoroutine;

        [Header("Animator Parameters")]
        private const string ANIMATOR_PARAM_IS_FIRING = "isFiring";
        private const string ANIMATOR_PARAM_IS_RELOADING = "isReloading";
        private const string ANIMATOR_PARAM_IS_AIMING = "isAiming";
        private const string ANIMATOR_PARAM_IS_FIRE_AIMING = "isFireAiming";
        private const string ANIMATOR_PARAM_IS_WALKING = "isWalking";
        private const string ANIMATOR_PARAM_WALK_AIMING = "isWalkAiming";

        public override void OnEnable()
        {
            _player.PlayerShoot.OnShootSuccessful += OnShootSuccessful;
            _player.PlayerShoot.OnReloadStarted += OnReloadStarted;
            _player.OnPlayerAimPressed += OnAimPressed;
            _player.OnPlayerAimReleased += OnAimReleased;
        }

        public override void OnDisable()
        {
            _player.PlayerShoot.OnShootSuccessful -= OnShootSuccessful;
            _player.PlayerShoot.OnReloadStarted -= OnReloadStarted;
            _player.OnPlayerAimPressed -= OnAimPressed;
            _player.OnPlayerAimReleased -= OnAimReleased;
        }

        private void OnShootSuccessful()
        {
            _player.StartCoroutine(PlayFireAnimation());
        }

        private void OnReloadStarted()
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

            if (_isWalking)
            {
                _armAnimator?.SetBool(ANIMATOR_PARAM_WALK_AIMING, isAiming);
            }
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
            _armAnimator?.SetBool(ANIMATOR_PARAM_WALK_AIMING, isWalking && _player.IsAiming);

            if (isWalking != _isWalking)
            {
                _isWalking = isWalking;

                if (_isWalking)
                {
                    StartFootsteps();
                    return;
                }
                StopFootsteps();
            }
        }

        private void StartFootsteps()
        {
            if (_footstepCoroutine != null)
            {
                _player.StopCoroutine(_footstepCoroutine);
            }
            _footstepCoroutine = _player.StartCoroutine(PlayFootstepsLoop());
        }

        private void StopFootsteps()
        {
            if (_footstepCoroutine != null)
            {
                _player.StopCoroutine(_footstepCoroutine);
                _footstepCoroutine = null;
            }
        }

        private IEnumerator PlayFootstepsLoop()
        {
            while (_isWalking)
            {
                if (_footstepSounds != null && _footstepSounds.Length > 0)
                {
                    AudioClip currentFootstep = _footstepSounds[UnityEngine.Random.Range(0, _footstepSounds.Length)];
                    
                    if (currentFootstep != null)
                    {
                        AudioManager.Instance.PlaySFX(currentFootstep, volume: 0.15f);
                    }
                }

                yield return new WaitForSeconds(_footstepInterval);
            }
        }
    }
}