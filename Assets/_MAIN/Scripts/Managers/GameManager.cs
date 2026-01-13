using RAIL_SHOOTER.PLAYER;
using UnityEngine;

namespace RAIL_SHOOTER.MANAGERS
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Shared Audio")]
        [SerializeField] private AudioClip[] _footstepSounds;
        private int _aliveEnemies = 0;
        [SerializeField] private string _endGameSceneName = "Menu";

        public AudioClip[] FootstepSounds => _footstepSounds;

        private int _screamingEnemies = 0;
        private PlayerController _playerController;

        public void RegisterPlayer(PlayerController player)
        {
            _playerController = player;
        }

        public void EnemyStartedScreaming()
        {
            _screamingEnemies++;
            if (_playerController != null)
            {
                _playerController.PlayerMovement?.SetMovementLocked(true);
            }
        }

        public void EnemyStoppedScreaming()
        {
            _screamingEnemies = Mathf.Max(0, _screamingEnemies - 1);
            if (_screamingEnemies == 0 && _playerController != null)
            {
                _playerController.PlayerMovement?.SetMovementLocked(false);
            }
        }

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
        public void RegisterEnemy()
        {
            _aliveEnemies++;
        }

        public void UnregisterEnemy()
        {
            _aliveEnemies = Mathf.Max(0, _aliveEnemies - 1);
            if (_aliveEnemies == 0)
            {
                EndGame();
            }
        }

        private void EndGame()
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(_endGameSceneName);
        }
        public AudioClip GetRandomFootstepSound()
        {
            if (_footstepSounds == null || _footstepSounds.Length == 0)
                return null;

            return _footstepSounds[Random.Range(0, _footstepSounds.Length)];
        }
    }
}