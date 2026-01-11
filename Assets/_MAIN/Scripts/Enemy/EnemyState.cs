namespace RAIL_SHOOTER.ENEMY
{
    public abstract class EnemyState
    {
        public abstract void EnterState(EnemyController enemy);
        public abstract void UpdateState();
        public abstract void ExitState();
    }
}