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

        private PlayerGun _playerGun;

        private void Start()
        {
            _player = GetComponent<PlayerController>();
            _playerGun = _player.PlayerGun;

            if (_playerGun != null)
            {
                _playerGun.OnAmmoChanged += UpdateAmmoDisplay;
                _playerGun.OnReloadStarted += OnReloadStarted;
                _playerGun.OnReloadFinished += OnReloadFinished;

                UpdateAmmoDisplay();
            }
        }

        private void OnDestroy()
        {
            _playerGun.OnAmmoChanged -= UpdateAmmoDisplay;
            _playerGun.OnReloadStarted -= OnReloadStarted;
            _playerGun.OnReloadFinished -= OnReloadFinished;
        }

        private void UpdateAmmoDisplay()
        {
            if (_ammoText != null && _playerGun != null)
            {
                _ammoText.text = $"{_playerGun.CurrentAmmo:00}/{_playerGun.MaxAmmo:00}";
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