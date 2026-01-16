using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using RAIL_SHOOTER.AUDIO;

namespace RAIL_SHOOTER.MENU
{
    public class Options : MenuState
    {
        [Header("Master Volume Controls")]
        [SerializeField] private Button _masterVolumeDownButton;
        [SerializeField] private Button _masterVolumeUpButton;
        [SerializeField] private Image[] _masterVolumeIndicators = new Image[7];

        [Header("SFX Volume Controls")]
        [SerializeField] private Button _sfxVolumeDownButton;
        [SerializeField] private Button _sfxVolumeUpButton;
        [SerializeField] private Image[] _sfxVolumeIndicators = new Image[7];

        [Header("Music Volume Controls")]
        [SerializeField] private Button _musicVolumeDownButton;
        [SerializeField] private Button _musicVolumeUpButton;
        [SerializeField] private Image[] _musicVolumeIndicators = new Image[7];

        [Header("Sensitivity Controls")]
        [SerializeField] private Button _sensitivityDownButton;
        [SerializeField] private Button _sensitivityUpButton;
        [SerializeField] private Image[] _sensitivityIndicators = new Image[7];

        [Header("Volume Indicator Colors")]
        [SerializeField] private Color _activeVolumeColor = Color.white;
        private readonly Color _inactiveVolumeColor = Color.gray;

        [Header("Other Options")]
        [SerializeField] private Button _resetDefaultsButton;

        private int _masterVolumeLevel = 5;
        private int _sfxVolumeLevel = 5;
        private int _musicVolumeLevel = 5;
        private int _sensitivityLevel = 5;
        
        [SerializeField] private AudioClip _buttonClick;

        public override void EnterState(MenuManager menuManager)
        {
            base.EnterState(menuManager);
            
            SetupControls();
            LoadSettings();
        }

        public override void ExitState()
        {
            base.ExitState();
            SaveSettings();
        }

        public override void UpdateState()
        {
        }

        private void SetupControls()
        {
            // Master Volume Controls
            if (_masterVolumeDownButton != null)
            {
                _masterVolumeDownButton.onClick.RemoveAllListeners();
                _masterVolumeDownButton.onClick.AddListener(() => AdjustVolume(VolumeType.Master, -1));
            }

            if (_masterVolumeUpButton != null)
            {
                _masterVolumeUpButton.onClick.RemoveAllListeners();
                _masterVolumeUpButton.onClick.AddListener(() => AdjustVolume(VolumeType.Master, 1));
            }

            // SFX Volume Controls
            if (_sfxVolumeDownButton != null)
            {
                _sfxVolumeDownButton.onClick.RemoveAllListeners();
                _sfxVolumeDownButton.onClick.AddListener(() => AdjustVolume(VolumeType.SFX, -1));
            }

            if (_sfxVolumeUpButton != null)
            {
                _sfxVolumeUpButton.onClick.RemoveAllListeners();
                _sfxVolumeUpButton.onClick.AddListener(() => AdjustVolume(VolumeType.SFX, 1));
            }

            // Music Volume Controls
            if (_musicVolumeDownButton != null)
            {
                _musicVolumeDownButton.onClick.RemoveAllListeners();
                _musicVolumeDownButton.onClick.AddListener(() => AdjustVolume(VolumeType.Music, -1));
            }

            if (_musicVolumeUpButton != null)
            {
                _musicVolumeUpButton.onClick.RemoveAllListeners();
                _musicVolumeUpButton.onClick.AddListener(() => AdjustVolume(VolumeType.Music, 1));
            }

            // Sensitivity Controls
            if (_sensitivityDownButton != null)
            {
                _sensitivityDownButton.onClick.RemoveAllListeners();
                _sensitivityDownButton.onClick.AddListener(() => AdjustSensitivity(-1));
            }

            if (_sensitivityUpButton != null)
            {
                _sensitivityUpButton.onClick.RemoveAllListeners();
                _sensitivityUpButton.onClick.AddListener(() => AdjustSensitivity(1));
            }

            // Reset button
            if (_resetDefaultsButton != null)
            {
                _resetDefaultsButton.onClick.RemoveAllListeners();
                _resetDefaultsButton.onClick.AddListener(ResetToDefaults);
            }
        }

        private void AdjustVolume(VolumeType volumeType, int adjustment)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    _masterVolumeLevel = Mathf.Clamp(_masterVolumeLevel + adjustment, 0, 7);
                    if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance)
                        RAIL_SHOOTER.AUDIO.AudioSettings.Instance.SetMasterVolume(_masterVolumeLevel);
                    break;

                case VolumeType.SFX:
                    _sfxVolumeLevel = Mathf.Clamp(_sfxVolumeLevel + adjustment, 0, 7);
                    if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance)
                        RAIL_SHOOTER.AUDIO.AudioSettings.Instance.SetSFXVolume(_sfxVolumeLevel);
                    break;

                case VolumeType.Music:
                    _musicVolumeLevel = Mathf.Clamp(_musicVolumeLevel + adjustment, 0, 7);
                    if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance)
                        RAIL_SHOOTER.AUDIO.AudioSettings.Instance.SetMusicVolume(_musicVolumeLevel);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(volumeType), volumeType, null);
            }

            UpdateVolumeDisplay();
            PlayButtonClickSFX();
        }

        private void UpdateVolumeDisplay()
        {
            // Update Master Volume indicators
            UpdateVolumeIndicators(_masterVolumeIndicators, _masterVolumeLevel);
            UpdateVolumeButtonStates(_masterVolumeDownButton, _masterVolumeUpButton, _masterVolumeLevel);

            // Update SFX Volume indicators
            UpdateVolumeIndicators(_sfxVolumeIndicators, _sfxVolumeLevel);
            UpdateVolumeButtonStates(_sfxVolumeDownButton, _sfxVolumeUpButton, _sfxVolumeLevel);

            // Update Music Volume indicators
            UpdateVolumeIndicators(_musicVolumeIndicators, _musicVolumeLevel);
            UpdateVolumeButtonStates(_musicVolumeDownButton, _musicVolumeUpButton, _musicVolumeLevel);

            // Update Sensitivity indicators
            UpdateSensitivityIndicators();
            UpdateSensitivityButtonStates();
        }

        private void UpdateVolumeIndicators(Image[] indicators, int volumeLevel)
        {
            for (var i = 0; i < indicators.Length; i++)
            {
                if (indicators[i])
                {
                    indicators[i].color = i < volumeLevel ? _activeVolumeColor : _inactiveVolumeColor;
                }
            }
        }

        private void UpdateVolumeButtonStates(Button downButton, Button upButton, int volumeLevel)
        {
            if (!downButton) return;
            downButton.interactable = volumeLevel > 0;

            if (!upButton) return;
            upButton.interactable = volumeLevel < 7;
        }

        private void LoadVolumeSettings()
        {
            if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
            {
                _masterVolumeLevel = RAIL_SHOOTER.AUDIO.AudioSettings.Instance.GetMasterVolume();
                _sfxVolumeLevel = RAIL_SHOOTER.AUDIO.AudioSettings.Instance.GetSFXVolume();
                _musicVolumeLevel = RAIL_SHOOTER.AUDIO.AudioSettings.Instance.GetMusicVolume();
            }
            else
            {
                _masterVolumeLevel = PlayerPrefs.GetInt("MasterVolume", 5);
                _sfxVolumeLevel = PlayerPrefs.GetInt("SFXVolume", 5);
                _musicVolumeLevel = PlayerPrefs.GetInt("MusicVolume", 5);
            }

            UpdateVolumeDisplay();
        }

        private void LoadSettings()
        {
            LoadVolumeSettings();
            LoadSensitivitySettings();
        }

        private void LoadSensitivitySettings()
        {
            _sensitivityLevel = PlayerPrefs.GetInt("MouseSensitivity", 5);
            UpdateVolumeDisplay(); // This will update sensitivity too
        }

        private void SaveVolumeSettings()
        {
        }

        private void SaveSettings()
        {
            SaveVolumeSettings();
            SaveSensitivitySettings();
        }

        private void SaveSensitivitySettings()
        {
            PlayerPrefs.SetInt("MouseSensitivity", _sensitivityLevel);
            PlayerPrefs.Save();
            Debug.Log($"[Options] Sensitivity saved: {_sensitivityLevel}");
        }

        private void AdjustSensitivity(int adjustment)
        {
            _sensitivityLevel = Mathf.Clamp(_sensitivityLevel + adjustment, 1, 7);
            SaveSensitivitySettings();
            UpdateVolumeDisplay(); // This will update sensitivity indicators too
            PlayButtonClickSFX();
            
            Debug.Log($"[Options] Sensitivity adjusted to: {_sensitivityLevel}");
        }

        private void UpdateSensitivityIndicators()
        {
            for (int i = 0; i < _sensitivityIndicators.Length; i++)
            {
                if (_sensitivityIndicators[i] != null)
                {
                    _sensitivityIndicators[i].color = i < _sensitivityLevel ? _activeVolumeColor : _inactiveVolumeColor;
                }
            }
        }

        private void UpdateSensitivityButtonStates()
        {
            if (_sensitivityDownButton != null)
                _sensitivityDownButton.interactable = _sensitivityLevel > 1;

            if (_sensitivityUpButton != null)
                _sensitivityUpButton.interactable = _sensitivityLevel < 7;
        }

        private void ResetToDefaults()
        {
            if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
            {
                RAIL_SHOOTER.AUDIO.AudioSettings.Instance.ResetToDefault();
                LoadVolumeSettings();
            }
            
            // Reset sensitivity to default
            _sensitivityLevel = 5;
            SaveSensitivitySettings();
            UpdateVolumeDisplay();
            PlayButtonClickSFX();
        }

        private void PlayButtonClickSFX()
        {
            AudioManager.Instance.PlaySFX(_buttonClick, 0.5f);
        }

        private void OnValidate()
        {
            if (_masterVolumeIndicators == null || _masterVolumeIndicators.Length != 7)
                _masterVolumeIndicators = new Image[7];

            if (_sfxVolumeIndicators == null || _sfxVolumeIndicators.Length != 7)
                _sfxVolumeIndicators = new Image[7];

            if (_musicVolumeIndicators == null || _musicVolumeIndicators.Length != 7)
                _musicVolumeIndicators = new Image[7];

            if (_sensitivityIndicators == null || _sensitivityIndicators.Length != 7)
                _sensitivityIndicators = new Image[7];
        }
        public static float GetMouseSensitivity()
        {
            int sensitivityLevel = PlayerPrefs.GetInt("MouseSensitivity", 5);
            return 0.5f + (sensitivityLevel - 1) * (1.5f / 6f);
        }

        private enum VolumeType
        {
            Master,
            SFX,
            Music
        }
    }
}