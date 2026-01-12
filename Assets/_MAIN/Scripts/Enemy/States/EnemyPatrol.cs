namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyPatrol : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
        }

        public override void ExitState()
        {
        }

        public override void UpdateState()
        {
        }
    }
}