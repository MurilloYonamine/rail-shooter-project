using UnityEngine;
using RAIL_SHOOTER.AUDIO;
using System;

namespace RAIL_SHOOTER.PLAYER
{
    [Serializable]
    public class PlayerAudio : PlayerComponent
    {
        [Header("Audio Clips")]
        [SerializeField] private AudioClip _fireSound;
        [SerializeField] private AudioClip _reloadSound;
        [SerializeField] private AudioClip _aimSound;
        [SerializeField] private AudioClip _emptyGunSound; // Som quando tenta atirar sem munição

        public override void OnEnable()
        {
            _player.PlayerShoot.OnShootSuccessful += OnShootSuccessful;
            _player.PlayerShoot.OnShootFailed += OnShootFailed;
            _player.PlayerShoot.OnReloadStarted += OnReloadStarted;
            _player.OnPlayerAimPressed += OnAimPressed;
            _player.OnPlayerAimReleased += OnAimReleased;
        }

        public override void OnDisable()
        {
            _player.PlayerShoot.OnShootSuccessful -= OnShootSuccessful;
            _player.PlayerShoot.OnShootFailed -= OnShootFailed;
            _player.PlayerShoot.OnReloadStarted -= OnReloadStarted;
            _player.OnPlayerAimPressed -= OnAimPressed;
            _player.OnPlayerAimReleased -= OnAimReleased;
        }

        private void OnShootSuccessful()
        {
            if(_fireSound == null) return;
            AudioManager.Instance.PlaySFX(_fireSound, volume: 0.4f);
        }

        private void OnShootFailed()
        {
            if(_emptyGunSound == null) return;
            AudioManager.Instance.PlaySFX(_emptyGunSound, volume: 0.3f);
        }

        private void OnReloadStarted()
        {
            if(_reloadSound == null) return;
            AudioManager.Instance.PlaySFX(_reloadSound, volume: 0.4f);
        }

        private void OnAimPressed()
        {
            if(_aimSound == null) return;
            AudioManager.Instance.PlaySFX(_aimSound, volume: 0.1f);
        }

        private void OnAimReleased()
        {
        }
    }
}