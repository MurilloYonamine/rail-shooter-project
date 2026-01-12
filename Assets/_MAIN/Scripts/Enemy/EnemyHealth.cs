using System;
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

        private void Awake()
        {
            _enemy = GetComponent<EnemyController>();
            CurrentHealth = MaxHealth;
        }
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth <= 0)
            {
                _enemy.ChangeState(_enemy.DeathState);
                CurrentHealth = 0;
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
            return CurrentHealth <= 0;
        }
    }
}