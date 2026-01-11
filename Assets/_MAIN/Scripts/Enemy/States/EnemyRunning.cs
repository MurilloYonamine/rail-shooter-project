namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyRunning : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.SetRunning(true);
        }

        public override void ExitState()
        {
            _enemy.Animator.SetRunning(false);
        }

        public override void UpdateState()
        {
            // Running movement logic here
        }
    }
}