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

        public override void OnEnable()
        {
            _player.OnPlayerFirePressed += OnFirePressed;
            _player.OnPlayerReloadPressed += OnReloadPressed;
            _player.OnPlayerAimPressed += OnAimPressed;
            _player.OnPlayerAimReleased += OnAimReleased;
        }

        public override void OnDisable()
        {
            _player.OnPlayerFirePressed -= OnFirePressed;
            _player.OnPlayerReloadPressed -= OnReloadPressed;
            _player.OnPlayerAimPressed -= OnAimPressed;
            _player.OnPlayerAimReleased -= OnAimReleased;
        }

        private void OnFirePressed()
        {
            if(_fireSound == null) return;
            AudioManager.Instance.PlaySFX(_fireSound, _player.transform.position, volume: 0.4f);
        }

        private void OnReloadPressed()
        {
            if(_reloadSound == null) return;
            AudioManager.Instance.PlaySFX(_reloadSound, _player.transform.position, volume: 0.4f);
        }

        private void OnAimPressed()
        {
            if(_aimSound == null) return;
            AudioManager.Instance.PlaySFX(_aimSound, _player.transform.position, volume: 0.1f);
        }

        private void OnAimReleased()
        {
        }
    }
}