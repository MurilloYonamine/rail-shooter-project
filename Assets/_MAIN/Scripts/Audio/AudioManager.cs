using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace RAIL_SHOOTER.AUDIO
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioMixer _audioMixer;

        [Header("Audio Mixer Groups")] [SerializeField]
        private AudioMixerGroup _masterMixerGroup;

        [SerializeField] private AudioMixerGroup _sfxMixerGroup;
        [SerializeField] private AudioMixerGroup _musicMixerGroup;

        [Header("Audio Mixer Parameters")] private const string MIXER_PARAM_MASTER_VOLUME = "MasterVolume";
        private const string MIXER_PARAM_SFX_VOLUME = "SFXVolume";
        private const string MIXER_PARAM_MUSIC_VOLUME = "MusicVolume";

        [Header("Audio Folder Paths")] private const string SFX_FOLDER_PATH = "Resources/Audio/SFX/";
        private const string MUSIC_FOLDER_PATH = "Resources/Audio/Music/";

        private GameObject _sfxAudioObject;
        private GameObject _musicAudioObject;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;

            transform.SetParent(null);
            DontDestroyOnLoad(gameObject);

            _sfxAudioObject = new GameObject("SFX");
            _sfxAudioObject.transform.parent = transform;

            _musicAudioObject = new GameObject("Music");
            _musicAudioObject.transform.parent = transform;
        }

        public void PlaySFX(AudioClip clip, float volume = 1f, float pitch = 1f, float blend = 0f)
        {
            var audioObject = CreateAudioObject(
                out var audioSource,
                $"Music - {clip.name}",
                _sfxAudioObject.transform
            );

            audioSource.clip = clip;
            audioSource.pitch = pitch;
            audioSource.volume = volume;
            audioSource.spatialBlend = 0f;
            audioSource.outputAudioMixerGroup = _sfxMixerGroup;
            audioSource.Play();

            Destroy(audioObject, clip.length);
        }

        public void PlayEnvironmentSFX(AudioClip clip, Vector3 position, float volume = 1f, bool loop = true)
        {
            var audioObject = CreateAudioObject(
                out var audioSource,
                $"Music - {clip.name}",
                _musicAudioObject.transform
            );

            audioObject.transform.position = position;

            audioSource.clip = clip;
            audioSource.loop = loop;
            audioSource.volume = volume;
            audioSource.spatialBlend = 1f;
            audioSource.minDistance = 1f;
            audioSource.maxDistance = 20f;
            audioSource.outputAudioMixerGroup = _sfxMixerGroup;
            audioSource.Play();

            if (loop) return;

            Destroy(audioObject, clip.length);
        }

        public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
        {
            StopAllMusic();

            var audioObject = CreateAudioObject(
                out var audioSource,
                $"Music - {clip.name}",
                _musicAudioObject.transform
            );

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.outputAudioMixerGroup = _musicMixerGroup;
            audioSource.Play();
        }

        public void StopMusic(string songName) => StopAudio(songName, _musicAudioObject);
        public void StopSFX(string sfxName) => StopAudio(sfxName, _sfxAudioObject);
        public void StopEnvironmentSFX(string sfxName) => StopAudio(sfxName, _sfxAudioObject);

        public void StopAllMusic()
        {
            var audioSources = _musicAudioObject.GetComponentsInChildren<AudioSource>();
            foreach (var source in audioSources)
            {
                if (source.isPlaying)
                {
                    Destroy(source.gameObject);
                }
            }
        }

        public void StopAllSFX()
        {
            var audioSources = _sfxAudioObject.GetComponentsInChildren<AudioSource>();
            foreach (var source in audioSources)
            {
                if (source.isPlaying)
                {
                    Destroy(source.gameObject);
                }
            }
        }

        public bool IsMusicPlaying()
        {
            var audioSources = _musicAudioObject.GetComponentsInChildren<AudioSource>();
            return audioSources.Any(source => source.isPlaying);
        }

        public AudioMixer GetAudioMixer() => _audioMixer;

        private void StopAudio(string audioName, GameObject parentObject)
        {
            var audioSources = parentObject.GetComponentsInChildren<AudioSource>();
            foreach (var source in audioSources)
            {
                if (source.clip.name != audioName) continue;

                Destroy(source.gameObject);
                break;
            }
        }

        private static GameObject CreateAudioObject(out AudioSource audioSource, string name, Transform parent = null)
        {
            var audioObject = new GameObject(name);
            audioObject.transform.parent = parent;

            audioSource = audioObject.AddComponent<AudioSource>();
            return audioObject;
        }
    }
}