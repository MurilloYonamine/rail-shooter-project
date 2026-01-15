using UnityEngine;
using RAIL_SHOOTER.PLAYER;
using System.Collections;

namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyAttack : EnemyState
    {
        
        public override void EnterState(EnemyController enemy)
        {
            base.EnterState(enemy);
        }

        public override void UpdateState()
        {

        }

        public override void ExitState()
        {
        }
    }
}