namespace RAIL_SHOOTER.ENEMY
{
    public abstract class EnemyState
    {
        protected EnemyController _enemy;

        public virtual void EnterState(EnemyController enemy)
        {
            _enemy = enemy;
        }
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}