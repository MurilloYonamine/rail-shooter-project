using UnityEngine;
using UnityEngine.Audio;

namespace RAIL_SHOOTER.AUDIO
{
    public class AudioSettings : MonoBehaviour
    {
        public static AudioSettings Instance { get; private set; }

        [Header("Audio Mixer")]
        [SerializeField] private AudioMixer mainAudioMixer;

        [Header("Volume Settings")]
        [Range(0, 7)] public int masterVolumeLevel = 5;
        [Range(0, 7)] public int sfxVolumeLevel = 5;
        [Range(0, 7)] public int musicVolumeLevel = 5;

        // PlayerPrefs keys
        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string SFX_VOLUME_KEY = "SFXVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            transform.parent = null;
            DontDestroyOnLoad(gameObject);
            
            LoadAudioSettings();
        }

        private void Start()
        {
            ApplyAllVolumeSettings();
        }

        public void SetMasterVolume(int level)
        {
            masterVolumeLevel = Mathf.Clamp(level, 0, 7);
            ApplyVolumeToMixer(masterVolumeLevel, MASTER_VOLUME_KEY);
            SaveAudioSettings();
        }

        public void SetSFXVolume(int level)
        {
            sfxVolumeLevel = Mathf.Clamp(level, 0, 7);
            ApplyVolumeToMixer(sfxVolumeLevel, SFX_VOLUME_KEY);
            SaveAudioSettings();
        }

        public void SetMusicVolume(int level)
        {
            musicVolumeLevel = Mathf.Clamp(level, 0, 7);
            ApplyVolumeToMixer(musicVolumeLevel, MUSIC_VOLUME_KEY);
            SaveAudioSettings();
        }

        private void ApplyVolumeToMixer(int volumeLevel, string parameterName)
        {
            if (!mainAudioMixer) return;

            float volumeDb;
            if (volumeLevel == 0)
            {
                volumeDb = -80f; // Mutado
            }
            else
            {
                volumeDb = Mathf.Lerp(-30f, 0f, (volumeLevel - 1) / 6f);
            }

            mainAudioMixer.SetFloat(parameterName, volumeDb);
        }

        private void ApplyAllVolumeSettings()
        {
            ApplyVolumeToMixer(masterVolumeLevel, MASTER_VOLUME_KEY);
            ApplyVolumeToMixer(sfxVolumeLevel, SFX_VOLUME_KEY);
            ApplyVolumeToMixer(musicVolumeLevel, MUSIC_VOLUME_KEY);
        }

        private void LoadAudioSettings()
        {
            masterVolumeLevel = PlayerPrefs.GetInt(MASTER_VOLUME_KEY, 5);
            sfxVolumeLevel = PlayerPrefs.GetInt(SFX_VOLUME_KEY, 5);
            musicVolumeLevel = PlayerPrefs.GetInt(MUSIC_VOLUME_KEY, 5);

            masterVolumeLevel = Mathf.Clamp(masterVolumeLevel, 0, 7);
            sfxVolumeLevel = Mathf.Clamp(sfxVolumeLevel, 0, 7);
            musicVolumeLevel = Mathf.Clamp(musicVolumeLevel, 0, 7);
        }

        private void SaveAudioSettings()
        {
            PlayerPrefs.SetInt(MASTER_VOLUME_KEY, masterVolumeLevel);
            PlayerPrefs.SetInt(SFX_VOLUME_KEY, sfxVolumeLevel);
            PlayerPrefs.SetInt(MUSIC_VOLUME_KEY, musicVolumeLevel);
            PlayerPrefs.Save();
        }
        public void ResetToDefault()
        {
            SetMasterVolume(6);
            SetSFXVolume(6);
            SetMusicVolume(6);
        }

        #region Public Getters
        public int GetMasterVolume() => masterVolumeLevel;
        public int GetSFXVolume() => sfxVolumeLevel;
        public int GetMusicVolume() => musicVolumeLevel;
        public AudioMixer GetAudioMixer() => mainAudioMixer;
        #endregion
    }
}