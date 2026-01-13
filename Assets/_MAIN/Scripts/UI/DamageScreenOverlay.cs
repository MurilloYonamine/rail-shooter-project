using UnityEngine;
using UnityEngine.UI;
using RAIL_SHOOTER.PLAYER;
using System.Collections;

namespace RAIL_SHOOTER.UI
{
    public class DamageScreenOverlay : MonoBehaviour
    {
        [Header("UI Components")]
        [SerializeField] private Image _overlayImage;
        [SerializeField] private Canvas _overlayCanvas;
        [Header("Overlay Text")]
        [SerializeField] private TMPro.TMP_Text _overlayText;

        [Header("Overlay Settings")]
        [SerializeField] private Color _overlayColor = new Color(1f, 0f, 0f, 0.5f);
        [SerializeField] private float _maxAlpha = 0.8f;
        [SerializeField, Range(0f, 100f)] private float _deathThreshold = 25f;
        [SerializeField] private float _fadeDuration = 0.5f;
        [SerializeField] private bool _enablePulseEffect = true;
        [SerializeField] private float _pulseSpeed = 2f;

        [Header("Death Settings")]
        [SerializeField] private float _deathScreenDuration = 3f;
        [SerializeField] private string _mainMenuSceneName = "MainMenu";

        [Header("End Game Settings")]
        [SerializeField] private Color _endGameOverlayColor = new Color(0f, 0f, 0f, 1f);
        [SerializeField] private float _endGameScreenDuration = 3f;

        private PlayerHealth _playerHealth;
        private float _targetAlpha = 0f;
        private bool _isPulsing = false;
        private Coroutine _pulseCoroutine;
        private Coroutine _fadeCoroutine;

        public static DamageScreenOverlay Instance { get; private set; }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
            SetupOverlay();
        }

        private void SetupOverlay()
        {
            _overlayColor.a = 0f;
            _overlayImage.color = _overlayColor;
        }

        public void SetPlayer(PlayerHealth player)
        {
            _playerHealth = player;
            if (_playerHealth != null)
            {
                UpdateOverlayFromHealth();
            }
            else
            {
                SetOverlayAlpha(0f);
            }
        }

        public void UpdateOverlayFromHealth()
        {
            if (_playerHealth == null || _playerHealth.IsDead())
            {
                SetOverlayAlpha(0f);
                return;
            }
            float healthPercentage = (float)_playerHealth.CurrentHealth / _playerHealth.MaxHealth;
            float damagePercentage = 1f - healthPercentage;
            float intensityMultiplier = Mathf.Pow(damagePercentage, 1.5f);
            _targetAlpha = intensityMultiplier * _maxAlpha;
            if (healthPercentage <= (_deathThreshold / 100f))
            {
                KillPlayer();
            }
            if (healthPercentage <= 0.4f && _enablePulseEffect && !_isPulsing)
            {
                StartPulseEffect();
            }
            else if (healthPercentage > 0.4f && _isPulsing)
            {
                StopPulseEffect();
            }
            if (_fadeCoroutine != null)
            {
                StopCoroutine(_fadeCoroutine);
            }
            _fadeCoroutine = StartCoroutine(FadeToAlpha(_targetAlpha));
        }

        private void SetOverlayAlpha(float alpha)
        {
            if (_overlayImage != null)
            {
                Color newColor = _overlayColor;
                newColor.a = alpha;
                _overlayImage.color = newColor;
            }
        }

        private IEnumerator FadeToAlpha(float targetAlpha)
        {
            float startAlpha = _overlayImage.color.a;
            float elapsedTime = 0f;
            while (elapsedTime < _fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float currentAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / _fadeDuration);
                SetOverlayAlpha(currentAlpha);
                yield return null;
            }
            SetOverlayAlpha(targetAlpha);
        }

        private void StartPulseEffect()
        {
            if (_isPulsing) return;
            _isPulsing = true;
            if (_pulseCoroutine != null)
            {
                StopCoroutine(_pulseCoroutine);
            }
            _pulseCoroutine = StartCoroutine(PulseEffect());
        }

        private void StopPulseEffect()
        {
            _isPulsing = false;
            if (_pulseCoroutine != null)
            {
                StopCoroutine(_pulseCoroutine);
                _pulseCoroutine = null;
            }
        }

        private IEnumerator PulseEffect()
        {
            while (_isPulsing)
            {
                float pulseIntensity = (Mathf.Sin(Time.time * _pulseSpeed) + 1f) * 0.5f;
                float pulseAlpha = _targetAlpha + (pulseIntensity * 0.2f);
                SetOverlayAlpha(Mathf.Clamp01(pulseAlpha));
                yield return null;
            }
        }

        public void ShowDeathScreen()
        {
            if (_overlayImage != null)
            {
                Color fullRed = new Color(1f, 0f, 0f, 1f);
                _overlayImage.color = fullRed;
            }
            if (_overlayText != null)
            {
                _overlayText.text = "Você morreu";
                _overlayText.gameObject.SetActive(true);
            }
            Time.timeScale = 0f;
            StartCoroutine(DeathScreenRoutineUnscaled());
        }

        private IEnumerator DeathScreenRoutineUnscaled()
        {
            float timer = 0f;
            while (timer < _deathScreenDuration)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
            Time.timeScale = 1f;
            UnityEngine.SceneManagement.SceneManager.LoadScene(_mainMenuSceneName);
        }

        public void ShowEndGameScreen()
        {
            if (_overlayImage != null)
            {
                _overlayImage.color = _endGameOverlayColor;
            }
            if (_overlayText != null)
            {
                _overlayText.text = "You Finished";
                _overlayText.gameObject.SetActive(true);
            }
            StartCoroutine(EndGameScreenRoutine());
        }

        private IEnumerator EndGameScreenRoutine()
        {
            float timer = 0f;
            while (timer < _endGameScreenDuration)
            {
                timer += Time.unscaledDeltaTime;
                yield return null;
            }
        }

        private void KillPlayer()
        {
            if (_playerHealth != null && !_playerHealth.IsDead())
            {
                _playerHealth.TakeDamage(_playerHealth.CurrentHealth);
                ShowDeathScreen();
            }
        }

        public void ForceUpdate()
        {
            UpdateOverlayFromHealth();
        }

        public void ClearOverlay()
        {
            SetPlayer(null);
            StopPulseEffect();
        }

        private void OnValidate()
        {
            _deathThreshold = Mathf.Clamp(_deathThreshold, 0f, 100f);
            _maxAlpha = Mathf.Clamp01(_maxAlpha);
        }
    }
}