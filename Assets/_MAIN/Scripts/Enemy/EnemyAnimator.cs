using System.Collections;
using UnityEngine;
using RAIL_SHOOTER.AUDIO;
using RAIL_SHOOTER.MANAGERS;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyAnimator
    {
        private const float ANIMATOR_PARAM_SPEED = 0.1f;
        private const string ANIMATOR_PARAM_ATTACK_SIMPLE = "AttackSimple";
        private const string ANIMATOR_PARAM_ATTACK_HEAVY = "AttackHeavy";
        private const string ANIMATOR_PARAM_IS_DEAD = "IsDead";
        private const string ANIMATOR_PARAM_ALERT = "Alert";
        private const string ANIMATOR_PARAM_HIT = "Hit";
        
        private Animator _animator;
        
        public EnemyAnimator(Animator animator)
        {
            _animator = animator;
        }
    }
}