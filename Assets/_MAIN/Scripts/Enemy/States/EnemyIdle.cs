namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyIdle : EnemyState
    {
        private EnemyController _enemy;
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.SetIdle(true);
        }

        public override void ExitState()
        {
            _enemy.Animator.SetIdle(false);
        }

        public override void UpdateState()
        {
        }
    }
}