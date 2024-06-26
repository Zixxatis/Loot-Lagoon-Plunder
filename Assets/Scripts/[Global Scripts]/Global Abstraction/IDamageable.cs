namespace CGames
{
    public interface IDamageable
    {
        public void TakeDamage(int damageAmount, bool isCritical = false);
    }
}