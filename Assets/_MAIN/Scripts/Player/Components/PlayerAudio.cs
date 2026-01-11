using UnityEngine;
using RAIL_SHOOTER.PLAYER.INPUT;
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
            InputReader.OnFirePressed += OnFirePressed;
            InputReader.OnReloadPressed += OnReloadPressed;
            InputReader.OnAimPressed += OnAimPressed;
            InputReader.OnAimReleased += OnAimReleased;
        }
        public override void OnDisable()
        {
            InputReader.OnFirePressed -= OnFirePressed;
            InputReader.OnReloadPressed -= OnReloadPressed;
            InputReader.OnAimPressed -= OnAimPressed;
            InputReader.OnAimReleased -= OnAimReleased;
        }
        private void OnFirePressed()
        {
            if(_fireSound == null) return;
            AudioManager.Instance.PlaySFX(_fireSound, _player.transform.position, volume: 0.7f);
        }
        private void OnReloadPressed()
        {
            if(_reloadSound == null) return;
            AudioManager.Instance.PlaySFX(_reloadSound, _player.transform.position);
        }
        private void OnAimPressed()
        {
            if(_aimSound == null) return;
            AudioManager.Instance.PlaySFX(_aimSound, _player.transform.position);
        }
        private void OnAimReleased()
        {
        }
    }
}