using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

namespace RAIL_SHOOTER.AUDIO
{
    public class AudioManager : MonoBehaviour
    {
        public static AudioManager Instance { get; private set; }

        [SerializeField] private AudioMixer _audioMixer;

        [Header("Audio Mixer Parameters")]
        private const string MIXER_PARAM_MASTER_VOLUME = "MasterVolume";
        private const string MIXER_PARAM_SFX_VOLUME = "SFXVolume";
        private const string MIXER_PARAM_MUSIC_VOLUME = "MusicVolume";

        [Header("Audio Folder Paths")]
        private const string SFX_FOLDER_PATH = "Resources/Audio/SFX/";
        private const string MUSIC_FOLDER_PATH = "Resources/Audio/Music/";

        private GameObject _sfxAudioObject;
        private GameObject _musicAudioObject;

        private void Awake()
        {
            if (Instance != null)
            {
                Destroy(gameObject);
            }
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        private void Start()
        {
            _sfxAudioObject = new GameObject("SFX");
            _sfxAudioObject.transform.parent = transform;

            _musicAudioObject = new GameObject("Music");
            _musicAudioObject.transform.parent = transform;
        }
        public void PlaySFX(AudioClip clip, Vector3 position, float volume = 1f)
        {
            GameObject audioObject = CreateAudioObject($"SFX - {clip.name}", _sfxAudioObject.transform);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = 1f;
            audioSource.Play();

            Destroy(audioObject, clip.length);
        }
        public void PlaySFX(string sfxName, Vector3 position, float volume = 1f)
        {
            string sfxPath = SFX_FOLDER_PATH + sfxName;
            AudioClip clip = Resources.Load<AudioClip>(sfxPath);

            GameObject audioObject = CreateAudioObject($"SFX - {clip.name}", _sfxAudioObject.transform);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.spatialBlend = 1f;
            audioSource.Play();

            Destroy(audioObject, clip.length);
        }
        public void PlayMusic(AudioClip clip, float volume = 1f, bool loop = true)
        {
            GameObject audioObject = CreateAudioObject($"Music - {clip.name}", _musicAudioObject.transform);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.Play();
        }
        public void PlayMusic(string songName, float volume = 1f, bool loop = true)
        {
            string songPath = MUSIC_FOLDER_PATH + songName;
            AudioClip clip = Resources.Load<AudioClip>(songPath);

            GameObject audioObject = CreateAudioObject($"Music - {clip.name}", _musicAudioObject.transform);
            AudioSource audioSource = audioObject.GetComponent<AudioSource>();

            audioSource.clip = clip;
            audioSource.volume = volume;
            audioSource.loop = loop;
            audioSource.Play();
        }
        public void StopMusic(string songName) => StopAudio(songName, _musicAudioObject);
        public void StopSFX(string sfxName) => StopAudio(sfxName, _sfxAudioObject);
        private void StopAudio(string audioName, GameObject parentObject)
        {
            AudioSource[] audioSources = parentObject.GetComponentsInChildren<AudioSource>();
            foreach (AudioSource source in audioSources)
            {
                if (source.clip.name == audioName)
                {
                    Destroy(source.gameObject);
                    break;
                }
            }
        }
        private GameObject CreateAudioObject(string name, Transform parent = null)
        {
            GameObject gameObject = new GameObject(name);
            gameObject.transform.parent = parent;
            gameObject.AddComponent<AudioSource>();

            return gameObject;
        }
    }
}