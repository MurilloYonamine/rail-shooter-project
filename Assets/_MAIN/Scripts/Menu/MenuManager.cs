using RAIL_SHOOTER.AUDIO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace RAIL_SHOOTER.MENU
{
    public enum MenuType
    {
        Principal,
        Pause
    }

    public class MenuManager : MonoBehaviour
    {
        [Header("Menu Configuration")]
        [SerializeField] private MenuType menuType = MenuType.Principal;

        [Header("Menu States")]
        [SerializeField] private MainMenu _mainMenu;
        [SerializeField] private Options _options;
        [SerializeField] private Credits _credits;
        [SerializeField] private Controls _controls;
        private MenuState _currentState;

        [Header("Main Menu Buttons")]
        [SerializeField] private Button _playButton;
        [SerializeField] private Button _optionsButton;
        [SerializeField] private Button _controlsButton;
        [SerializeField] private Button _creditsButton;
        [SerializeField] private Button _quitButton;

        [Header("Return Button")]
        [SerializeField] private Button _returnButton;

        [Header("Audio")]
        [SerializeField] private AudioClip _menuMusicClip;
        [SerializeField] private AudioClip _buttonHoverSound;
        [SerializeField] private AudioClip _buttonClickSound;

        [Header("Scene Management")]
        [SerializeField] private string _gameplaySceneName = "MainScene";

        private void Start()
        {
            ChangeState(_mainMenu);
            SetupButtons();
            
            if (menuType == MenuType.Principal)
            {
                AudioManager.Instance.PlayMusic(_menuMusicClip);
            }
        }

        private void Update()
        {
            if (menuType == MenuType.Pause && Input.GetKeyDown(KeyCode.Escape))
            {
                ReturnToGame();
            }
        }

        private void SetupButtons()
        {
            _playButton.onClick.AddListener(LoadGameplay);
            _optionsButton.onClick.AddListener(() => ChangeState(_options));
            _controlsButton.onClick.AddListener(() => ChangeState(_controls));
            _creditsButton.onClick.AddListener(() => ChangeState(_credits));
            _quitButton.onClick.AddListener(Exit);
            _returnButton.onClick.AddListener(ReturnToMainMenu);

            AddHoverComponentIfMissing(_returnButton);
            AddHoverComponentIfMissing(_playButton);
            AddHoverComponentIfMissing(_optionsButton);
            AddHoverComponentIfMissing(_controlsButton);
            AddHoverComponentIfMissing(_creditsButton);
            AddHoverComponentIfMissing(_quitButton);
        }

        private void AddHoverComponentIfMissing(Button button)
        {
            if (button != null && button.GetComponent<MenuButtonHover>() == null)
            {
                MenuButtonHover hoverComponent = button.gameObject.AddComponent<MenuButtonHover>();

                if (hoverComponent != null)
                {
                    hoverComponent.SetAudioClips(_buttonHoverSound, _buttonClickSound);
                }
            }
        }

        public void ChangeState(MenuState newState)
        {
            _currentState?.ExitState();
            _currentState = newState;
            _currentState.EnterState(this);

            UpdateReturnButtonVisibility();

            ResetAllButtonHovers();
        }

        private void UpdateReturnButtonVisibility()
        {
            if (_returnButton != null)
            {
                bool shouldShowReturnButton = _currentState != _mainMenu;
                _returnButton.gameObject.SetActive(shouldShowReturnButton);
            }
        }

        private void ResetAllButtonHovers()
        {
            Button[] allButtons = { _playButton, _optionsButton, _controlsButton, _creditsButton, _quitButton, _returnButton };

            foreach (var button in allButtons)
            {
                if (button != null)
                {
                    MenuButtonHover hoverComponent = button.GetComponent<MenuButtonHover>();
                    if (hoverComponent != null)
                    {
                        hoverComponent.ResetHover();
                    }
                }
            }
        }

        public void ReturnToMainMenu()
        {
            ChangeState(_mainMenu);
        }

        public void ReturnToGame()
        {
            if (menuType == MenuType.Pause)
            {
                gameObject.SetActive(false);
                Time.timeScale = 1f; 
                Debug.Log("[MenuManager] Voltando para o jogo");
            }
        }

        public void LoadGameplay()
        {
            Debug.Log("[MenuManager] Loading gameplay scene: " + _gameplaySceneName);

            AudioManager.Instance.StopAllMusic();

            SceneManager.LoadScene(_gameplaySceneName);
        }

        private void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
        }

        // Getters
        public MenuType GetMenuType() => menuType;
        public void SetMenuType(MenuType type) => menuType = type;

        #region State Getters
        public MainMenu MainMenuState => _mainMenu;
        public Options OptionsState => _options;
        public Credits CreditsState => _credits;
        public Controls ControlsState => _controls;
        public MenuState CurrentState => _currentState;
        #endregion
    }
}