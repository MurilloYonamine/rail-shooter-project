namespace RAIL_SHOOTER.ENEMY
{
    public class EnemyAttack : EnemyState
    {
        private EnemyController _enemy;
        
        public override void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
            _enemy.Animator.PlayAttackAnimation();
        }

        public override void ExitState()
        {
            // Animation will finish naturally
        }

        public override void UpdateState()
        {
            // Attack logic here - for now just return to idle after a delay
            // In a real game, you'd check if attack animation is finished
        }
    }
}