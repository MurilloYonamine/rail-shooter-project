namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyDeath : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.PlayDeathAnimation();
        }

        public override void ExitState()
        {
            // Usually won't exit from death state
        }

        public override void UpdateState()
        {
            // Death logic here - maybe destroy object after animation
        }
    }
}