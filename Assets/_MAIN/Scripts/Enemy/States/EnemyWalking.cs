namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyWalking : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.SetWalking(true);
        }

        public override void ExitState()
        {
            _enemy.Animator.SetWalking(false);
        }

        public override void UpdateState()
        {
            // Walking movement logic here
        }
    }
}