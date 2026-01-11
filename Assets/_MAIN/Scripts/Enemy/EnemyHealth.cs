using System;
using RAIL_SHOOTER.UTILITIES;
using UnityEngine;

namespace RAIL_SHOOTER.ENEMY
{
    [Serializable]
    public class EnemyHealth : IHealth
    {
        public int CurrentHealth { get; set; }
        public int MaxHealth { get; set; }

        public EnemyHealth(int maxHealth)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
        }
        public void TakeDamage(int damage)
        {
            CurrentHealth -= damage;
            if (CurrentHealth < 0)
            {
                CurrentHealth = 0;
            }
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