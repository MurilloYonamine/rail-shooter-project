using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using RAIL_SHOOTER.AUDIO;

namespace RAIL_SHOOTER.MENU
{
    public class Options : MenuState
    {
        [Header("Master Volume Controls")]
        [SerializeField] private Button masterVolumeDownButton;
        [SerializeField] private Button masterVolumeUpButton;
        [SerializeField] private Image[] masterVolumeIndicators = new Image[7];

        [Header("SFX Volume Controls")]
        [SerializeField] private Button sfxVolumeDownButton;
        [SerializeField] private Button sfxVolumeUpButton;
        [SerializeField] private Image[] sfxVolumeIndicators = new Image[7];

        [Header("Music Volume Controls")]
        [SerializeField] private Button musicVolumeDownButton;
        [SerializeField] private Button musicVolumeUpButton;
        [SerializeField] private Image[] musicVolumeIndicators = new Image[7];

        [Header("Sensitivity Controls")]
        [SerializeField] private Button sensitivityDownButton;
        [SerializeField] private Button sensitivityUpButton;
        [SerializeField] private Image[] sensitivityIndicators = new Image[7];

        [Header("Volume Indicator Colors")]
        [SerializeField] private Color activeVolumeColor = Color.white;
        [SerializeField] private Color inactiveVolumeColor = Color.gray;

        [Header("Other Options")]
        [SerializeField] private Button resetDefaultsButton;

        private int masterVolumeLevel = 5;
        private int sfxVolumeLevel = 5;
        private int musicVolumeLevel = 5;
        private int sensitivityLevel = 5;

        public override void EnterState(MenuManager menuManager)
        {
            base.EnterState(menuManager);
            _menuManager = menuManager;
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
            if (masterVolumeDownButton != null)
            {
                masterVolumeDownButton.onClick.RemoveAllListeners();
                masterVolumeDownButton.onClick.AddListener(() => AdjustVolume(VolumeType.Master, -1));
            }

            if (masterVolumeUpButton != null)
            {
                masterVolumeUpButton.onClick.RemoveAllListeners();
                masterVolumeUpButton.onClick.AddListener(() => AdjustVolume(VolumeType.Master, 1));
            }

            // SFX Volume Controls
            if (sfxVolumeDownButton != null)
            {
                sfxVolumeDownButton.onClick.RemoveAllListeners();
                sfxVolumeDownButton.onClick.AddListener(() => AdjustVolume(VolumeType.SFX, -1));
            }

            if (sfxVolumeUpButton != null)
            {
                sfxVolumeUpButton.onClick.RemoveAllListeners();
                sfxVolumeUpButton.onClick.AddListener(() => AdjustVolume(VolumeType.SFX, 1));
            }

            // Music Volume Controls
            if (musicVolumeDownButton != null)
            {
                musicVolumeDownButton.onClick.RemoveAllListeners();
                musicVolumeDownButton.onClick.AddListener(() => AdjustVolume(VolumeType.Music, -1));
            }

            if (musicVolumeUpButton != null)
            {
                musicVolumeUpButton.onClick.RemoveAllListeners();
                musicVolumeUpButton.onClick.AddListener(() => AdjustVolume(VolumeType.Music, 1));
            }

            // Sensitivity Controls
            if (sensitivityDownButton != null)
            {
                sensitivityDownButton.onClick.RemoveAllListeners();
                sensitivityDownButton.onClick.AddListener(() => AdjustSensitivity(-1));
            }

            if (sensitivityUpButton != null)
            {
                sensitivityUpButton.onClick.RemoveAllListeners();
                sensitivityUpButton.onClick.AddListener(() => AdjustSensitivity(1));
            }

            // Reset button
            if (resetDefaultsButton != null)
            {
                resetDefaultsButton.onClick.RemoveAllListeners();
                resetDefaultsButton.onClick.AddListener(ResetToDefaults);
            }
        }

        private void AdjustVolume(VolumeType volumeType, int adjustment)
        {
            switch (volumeType)
            {
                case VolumeType.Master:
                    masterVolumeLevel = Mathf.Clamp(masterVolumeLevel + adjustment, 0, 7);
                    if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
                        RAIL_SHOOTER.AUDIO.AudioSettings.Instance.SetMasterVolume(masterVolumeLevel);
                    break;

                case VolumeType.SFX:
                    sfxVolumeLevel = Mathf.Clamp(sfxVolumeLevel + adjustment, 0, 7);
                    if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
                        RAIL_SHOOTER.AUDIO.AudioSettings.Instance.SetSFXVolume(sfxVolumeLevel);
                    break;

                case VolumeType.Music:
                    musicVolumeLevel = Mathf.Clamp(musicVolumeLevel + adjustment, 0, 7);
                    if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
                        RAIL_SHOOTER.AUDIO.AudioSettings.Instance.SetMusicVolume(musicVolumeLevel);
                    break;
            }

            UpdateVolumeDisplay();
            PlayButtonClickSFX();
        }

        private void UpdateVolumeDisplay()
        {
            // Update Master Volume indicators
            UpdateVolumeIndicators(masterVolumeIndicators, masterVolumeLevel);
            UpdateVolumeButtonStates(masterVolumeDownButton, masterVolumeUpButton, masterVolumeLevel);

            // Update SFX Volume indicators
            UpdateVolumeIndicators(sfxVolumeIndicators, sfxVolumeLevel);
            UpdateVolumeButtonStates(sfxVolumeDownButton, sfxVolumeUpButton, sfxVolumeLevel);

            // Update Music Volume indicators
            UpdateVolumeIndicators(musicVolumeIndicators, musicVolumeLevel);
            UpdateVolumeButtonStates(musicVolumeDownButton, musicVolumeUpButton, musicVolumeLevel);

            // Update Sensitivity indicators
            UpdateSensitivityIndicators();
            UpdateSensitivityButtonStates();
        }

        private void UpdateVolumeIndicators(Image[] indicators, int volumeLevel)
        {
            for (int i = 0; i < indicators.Length; i++)
            {
                if (indicators[i] != null)
                {
                    indicators[i].color = i < volumeLevel ? activeVolumeColor : inactiveVolumeColor;
                }
            }
        }

        private void UpdateVolumeButtonStates(Button downButton, Button upButton, int volumeLevel)
        {
            if (downButton != null)
                downButton.interactable = volumeLevel > 0;

            if (upButton != null)
                upButton.interactable = volumeLevel < 7;
        }

        private void LoadVolumeSettings()
        {
            if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
            {
                masterVolumeLevel = RAIL_SHOOTER.AUDIO.AudioSettings.Instance.GetMasterVolume();
                sfxVolumeLevel = RAIL_SHOOTER.AUDIO.AudioSettings.Instance.GetSFXVolume();
                musicVolumeLevel = RAIL_SHOOTER.AUDIO.AudioSettings.Instance.GetMusicVolume();
            }
            else
            {
                masterVolumeLevel = PlayerPrefs.GetInt("MasterVolume", 5);
                sfxVolumeLevel = PlayerPrefs.GetInt("SFXVolume", 5);
                musicVolumeLevel = PlayerPrefs.GetInt("MusicVolume", 5);
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
            sensitivityLevel = PlayerPrefs.GetInt("MouseSensitivity", 5);
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
            PlayerPrefs.SetInt("MouseSensitivity", sensitivityLevel);
            PlayerPrefs.Save();
            Debug.Log($"[Options] Sensitivity saved: {sensitivityLevel}");
        }

        private void AdjustSensitivity(int adjustment)
        {
            sensitivityLevel = Mathf.Clamp(sensitivityLevel + adjustment, 1, 7);
            SaveSensitivitySettings();
            UpdateVolumeDisplay(); // This will update sensitivity indicators too
            PlayButtonClickSFX();
            
            Debug.Log($"[Options] Sensitivity adjusted to: {sensitivityLevel}");
        }

        private void UpdateSensitivityIndicators()
        {
            for (int i = 0; i < sensitivityIndicators.Length; i++)
            {
                if (sensitivityIndicators[i] != null)
                {
                    sensitivityIndicators[i].color = i < sensitivityLevel ? activeVolumeColor : inactiveVolumeColor;
                }
            }
        }

        private void UpdateSensitivityButtonStates()
        {
            if (sensitivityDownButton != null)
                sensitivityDownButton.interactable = sensitivityLevel > 1;

            if (sensitivityUpButton != null)
                sensitivityUpButton.interactable = sensitivityLevel < 7;
        }

        private void ResetToDefaults()
        {
            if (RAIL_SHOOTER.AUDIO.AudioSettings.Instance != null)
            {
                RAIL_SHOOTER.AUDIO.AudioSettings.Instance.ResetToDefault();
                LoadVolumeSettings();
            }
            
            // Reset sensitivity to default
            sensitivityLevel = 5;
            SaveSensitivitySettings();
            UpdateVolumeDisplay();
            PlayButtonClickSFX();
        }

        private void PlayButtonClickSFX()
        {
            AudioManager.Instance.PlaySFX("button_click", 0.5f);
        }

        private void OnValidate()
        {
            if (masterVolumeIndicators == null || masterVolumeIndicators.Length != 7)
                masterVolumeIndicators = new Image[7];

            if (sfxVolumeIndicators == null || sfxVolumeIndicators.Length != 7)
                sfxVolumeIndicators = new Image[7];

            if (musicVolumeIndicators == null || musicVolumeIndicators.Length != 7)
                musicVolumeIndicators = new Image[7];

            if (sensitivityIndicators == null || sensitivityIndicators.Length != 7)
                sensitivityIndicators = new Image[7];
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