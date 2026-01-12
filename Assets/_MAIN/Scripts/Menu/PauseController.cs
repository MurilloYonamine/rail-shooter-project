using UnityEngine;
using RAIL_SHOOTER.MENU;
using RAIL_SHOOTER.PLAYER;

namespace RAIL_SHOOTER.MENU
{
    public class PauseController : MonoBehaviour
    {
        [Header("Pause Configuration")]
        [SerializeField] private MenuManager _pauseMenuManager;
        [SerializeField] private PlayerController _playerController;
        
        private bool _isPaused = false;
        
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                TogglePause();
            }
        }
        
        private void TogglePause()
        {
            if (_pauseMenuManager == null) return;
            
            if (!_isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }
        
        private void PauseGame()
        {
            _isPaused = true;
            Time.timeScale = 0f; 
            
            if (_playerController != null)
            {
                _playerController.enabled = false;
            }
            
            _pauseMenuManager.ShowPauseMenu();
        }
        
        private void ResumeGame()
        {
            _isPaused = false;
            Time.timeScale = 1f; 
            
            if (_playerController != null)
            {
                _playerController.enabled = true;
            }
            
            _pauseMenuManager.ReturnToGame();
        }
        
        public void RequestResume()
        {
            if (_isPaused)
            {
                ResumeGame();
            }
        }
    }
}