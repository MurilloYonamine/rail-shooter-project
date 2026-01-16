using System;
using System.Collections;
using UnityEngine;
using RAIL_SHOOTER.AUDIO;
using RAIL_SHOOTER.MANAGERS;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerAnimator : PlayerComponent
    {
        [Header("Animators")]
        [SerializeField] private Animator _armAnimator;
        [SerializeField] private Animator _gunAnimator;

        [Header("Footstep Audio")]
        [SerializeField] private float _footstepInterval = 0.5f;

        private bool _isWalking = false;
        private Coroutine _footstepCoroutine;

        [Header("Animator Parameters")]
        private const string ANIMATOR_PARAM_FIRE = "Fire";
        private const string ANIMATOR_PARAM_RELOAD = "Reload";
        private const string ANIMATOR_PARAM_IS_AIMING = "IsAiming";
        private const string ANIMATOR_PARAM_SPEED = "Speed";

        public override void OnEnable()
        {
            _player.PlayerGun.OnShootSuccessful += OnGunSuccessful;
            _player.PlayerGun.OnReloadStarted += OnReloadStarted;
            _player.OnPlayerAimPressed += OnAimPressed;
            _player.OnPlayerAimReleased += OnAimReleased;
        }

        public override void OnDisable()
        {
            _player.PlayerGun.OnShootSuccessful -= OnGunSuccessful;
            _player.PlayerGun.OnReloadStarted -= OnReloadStarted;
            _player.OnPlayerAimPressed -= OnAimPressed;
            _player.OnPlayerAimReleased -= OnAimReleased;
        }

        private void OnGunSuccessful()
        {
            PlayFireAnimation();
        }

        private void OnReloadStarted()
        {
            PlayReloadAnimation();
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

        private void PlayFireAnimation()
        {
            _player.SetFiring(true);

            _armAnimator?.SetTrigger(ANIMATOR_PARAM_FIRE);
            _gunAnimator?.SetTrigger(ANIMATOR_PARAM_FIRE);

            _player.SetFiring(false);
        }

        private void PlayReloadAnimation()
        {
            _player.SetReloading(true);

            _armAnimator?.SetTrigger(ANIMATOR_PARAM_RELOAD);
            _gunAnimator?.SetTrigger(ANIMATOR_PARAM_RELOAD);

            _player.SetReloading(false);
        }
        public void SetWalking(bool isWalking, float speed)
        {
            _armAnimator?.SetFloat(ANIMATOR_PARAM_SPEED, speed);
            
            if (isWalking == _isWalking) return;
            
            _isWalking = isWalking;

            if (_isWalking)
            {
                StartFootsteps();
                return;
            }
            StopFootsteps();
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
            if (_footstepCoroutine == null) return;
            
            _player.StopCoroutine(_footstepCoroutine);
            _footstepCoroutine = null;
        }

        private IEnumerator PlayFootstepsLoop()
        {
            while (_isWalking)
            {
                if (GameManager.Instance)
                {
                    var currentFootstep = GameManager.Instance.GetRandomFootstepSound();
                    
                    if (currentFootstep && AudioManager.Instance)
                    {
                        try
                        {
                            AudioManager.Instance.PlaySFX(currentFootstep, volume: 0.15f);
                        }
                        catch (System.Exception e)
                        {
                            Debug.LogWarning($"[PlayerAnimator] AudioManager não configurado corretamente: {e.Message}");
                        }
                    }
                }

                yield return new WaitForSeconds(_footstepInterval);
            }
        }
    }
}