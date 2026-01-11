namespace RAIL_SHOOTER.UTILITIES
{

    public interface IHealth
    {
        int CurrentHealth { get; }
        int MaxHealth { get; }
        void TakeDamage(int damage);
        void Heal(int amount);
        bool IsDead();
    }
}