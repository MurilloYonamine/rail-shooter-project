using System;
using System.Collections;
using RAIL_SHOOTER.UTILITIES;
using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    [RequireComponent(typeof(EnemyController))]
    public class EnemyHealth : MonoBehaviour, IHealth
    {
        private EnemyController _enemy;
        [field: SerializeField] public int CurrentHealth { get; set; }
        [field: SerializeField] public int MaxHealth { get; set; }
        private Collider _collider;

        [SerializeField] private float _timeToDisable = 9f;

        private void Awake()
        {
            _collider = GetComponent<Collider>();
            _enemy = GetComponent<EnemyController>();
            CurrentHealth = MaxHealth;
        }
        public void TakeDamage(int damage)
        {
            if (!_enemy.CanTakeHit)
            {
                return;
            }
            
            _enemy.RegisterHit();
            CurrentHealth -= damage;

            if (IsDead())
            {
                _enemy.EnablePlayerMovement();
                _enemy.ChangeState(_enemy.DeathState);
                CurrentHealth = 0;
                StartCoroutine(DisableObject());
                return;
            }

            _enemy.ChangeState(_enemy.EnemyHit);
        }
        public void Heal(int amount)
        {
            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth)
            {
                CurrentHealth = MaxHealth;
            }
        }
        public bool IsDead()
        {
            if (CurrentHealth <= 0)
            {
                _collider.enabled = false;
                return true;
            }
            return false;
        }
        private IEnumerator DisableObject()
        {
            yield return new WaitForSeconds(_timeToDisable);
            gameObject.SetActive(false);
        }
    }
}