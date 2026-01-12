using UnityEngine;
using TMPro;
using RAIL_SHOOTER.PLAYER;

namespace RAIL_SHOOTER.UI
{
    [RequireComponent(typeof(PlayerController))]
    public class AmmoUI : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private TextMeshProUGUI _ammoText;

        [Header("Player Reference")]
        [SerializeField] private PlayerController _player;

        private PlayerShoot _playerShoot;

        private void Start()
        {
            _player = GetComponent<PlayerController>();
            _playerShoot = _player.PlayerShoot;

            if (_playerShoot != null)
            {
                _playerShoot.OnAmmoChanged += UpdateAmmoDisplay;
                _playerShoot.OnReloadStarted += OnReloadStarted;
                _playerShoot.OnReloadFinished += OnReloadFinished;

                UpdateAmmoDisplay();
            }
        }

        private void OnDestroy()
        {
            _playerShoot.OnAmmoChanged -= UpdateAmmoDisplay;
            _playerShoot.OnReloadStarted -= OnReloadStarted;
            _playerShoot.OnReloadFinished -= OnReloadFinished;
        }

        private void UpdateAmmoDisplay()
        {
            if (_ammoText != null && _playerShoot != null)
            {
                _ammoText.text = $"{_playerShoot.CurrentAmmo:00}/{_playerShoot.MaxAmmo:00}";
            }
        }

        private void OnReloadStarted()
        {

        }

        private void OnReloadFinished()
        {
            UpdateAmmoDisplay();
        }
    }
}