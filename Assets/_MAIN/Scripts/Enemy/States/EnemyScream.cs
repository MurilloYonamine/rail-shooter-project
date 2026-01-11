namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyScream : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.PlayScreamAnimation();
        }

        public override void ExitState()
        {
            // Scream animation will finish naturally
        }

        public override void UpdateState()
        {
            // Scream logic here
        }
    }
}