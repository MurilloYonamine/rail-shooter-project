using UnityEngine;

namespace RAIL_SHOOTER.MANAGERS
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }
        
        [Header("Shared Audio")]
        [SerializeField] private AudioClip[] _footstepSounds;
        
        public AudioClip[] FootstepSounds => _footstepSounds;
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                if (transform.parent == null)
                {
                    DontDestroyOnLoad(gameObject);
                }
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        public AudioClip GetRandomFootstepSound()
        {
            if (_footstepSounds == null || _footstepSounds.Length == 0)
                return null;
                
            return _footstepSounds[Random.Range(0, _footstepSounds.Length)];
        }
    }
}