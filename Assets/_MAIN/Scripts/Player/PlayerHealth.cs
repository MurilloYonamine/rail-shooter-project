using System;
using System.Collections;
using RAIL_SHOOTER.UTILITIES;
using RAIL_SHOOTER.UI;
using UnityEngine;

namespace RAIL_SHOOTER.PLAYER
{
    public class PlayerHealth : MonoBehaviour, IHealth
    {
        [field: SerializeField] public int CurrentHealth { get; set; } = 100;
        [field: SerializeField] public int MaxHealth { get; set; } = 100;
        
        [Header("Damage Settings")]
        [SerializeField, Range(0.1f, 2f)] private float _damageCooldown = 0.5f;
        [SerializeField] private bool _canTakeDamage = true;
        
        [Header("Death Settings")]
        [SerializeField] private float _timeToRespawn = 3f;
        [SerializeField] private bool _autoRespawn = true;
        
        [Header("Regen Settings")]
        [SerializeField] private float _regenDelay = 3f;
        [SerializeField] private float _regenRate = 10f; // pontos de vida por segundo
        private float _regenTimer = 0f;
        private bool _isRegenerating = false;
        
        private float _lastDamageTime = -1f;
        private bool _isDead = false;
        
        public bool CanTakeDamage => _canTakeDamage && Time.time >= _lastDamageTime + _damageCooldown;
        public bool IsInvulnerable => !_canTakeDamage;
        
        public event Action<int, int> OnHealthChanged;
        public event Action<int> OnDamageTaken;
        public event Action<int> OnHealed;
        public event Action OnPlayerDied;
        public event Action OnPlayerRespawned;
        
        private void Awake()
        {
            CurrentHealth = MaxHealth;
        }
        
        private void Start()
        {
            if (DamageScreenOverlay.Instance != null)
            {
                DamageScreenOverlay.Instance.SetPlayer(this);
            }
            
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        private void Update()
        {
            if (!_isDead && CurrentHealth < MaxHealth)
            {
                if (_isRegenerating)
                {
                    CurrentHealth += Mathf.CeilToInt(_regenRate * Time.deltaTime);
                    if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;
                    OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
                    if (DamageScreenOverlay.Instance != null)
                    {
                        DamageScreenOverlay.Instance.UpdateOverlayFromHealth();
                    }
                    if (CurrentHealth == MaxHealth) _isRegenerating = false;
                }
                else
                {
                    _regenTimer += Time.deltaTime;
                    if (_regenTimer >= _regenDelay)
                    {
                        _isRegenerating = true;
                    }
                }
            }
        }
        
        public void TakeDamage(int damage)
        {
            if (!CanTakeDamage || _isDead || damage <= 0)
            {
                return;
            }
            
            _lastDamageTime = Time.time;
            int previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
            
            OnDamageTaken?.Invoke(damage);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            
            if (DamageScreenOverlay.Instance != null)
            {
                DamageScreenOverlay.Instance.UpdateOverlayFromHealth();
            }
            
            if (IsDead())
            {
                HandleDeath();
            }
            
            _regenTimer = 0f;
            _isRegenerating = false;
        }
        
        public void Heal(int amount)
        {
            if (_isDead || amount <= 0)
            {
                return;
            }
            
            int previousHealth = CurrentHealth;
            CurrentHealth = Mathf.Min(MaxHealth, CurrentHealth + amount);
            
            OnHealed?.Invoke(amount);
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
            
            if (DamageScreenOverlay.Instance != null)
            {
                DamageScreenOverlay.Instance.UpdateOverlayFromHealth();
            }
            if (CurrentHealth == MaxHealth)
            {
                _isRegenerating = false;
            }
        }
        
        public bool IsDead()
        {
            return CurrentHealth <= 0;
        }
        
        private void HandleDeath()
        {
            if (_isDead) return;
            
            _isDead = true;
            _canTakeDamage = false;
            
            OnPlayerDied?.Invoke();
            
            if (_autoRespawn)
            {
                StartCoroutine(RespawnCoroutine());
            }
        }
        
        private IEnumerator RespawnCoroutine()
        {
            yield return new WaitForSeconds(_timeToRespawn);
            Respawn();
        }
        
        public void Respawn()
        {
            _isDead = false;
            _canTakeDamage = true;
            CurrentHealth = MaxHealth;
            
            if (DamageScreenOverlay.Instance != null)
            {
                DamageScreenOverlay.Instance.UpdateOverlayFromHealth();
            }
            
            OnPlayerRespawned?.Invoke();
            OnHealthChanged?.Invoke(CurrentHealth, MaxHealth);
        }
        
        public void SetInvulnerable(bool invulnerable)
        {
            _canTakeDamage = !invulnerable;
        }
        
        public void SetTemporaryInvulnerability(float duration)
        {
            StartCoroutine(TemporaryInvulnerabilityCoroutine(duration));
        }
        
        private IEnumerator TemporaryInvulnerabilityCoroutine(float duration)
        {
            bool wasInvulnerable = !_canTakeDamage;
            SetInvulnerable(true);
            
            yield return new WaitForSeconds(duration);
            
            if (!wasInvulnerable)
            {
                SetInvulnerable(false);
            }
        }
    
    }
}