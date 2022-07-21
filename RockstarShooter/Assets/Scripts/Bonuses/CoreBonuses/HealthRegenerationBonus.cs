namespace Bonuses.CoreBonuses
{
    public class HealthRegenerationBonus : IBonus
    {
        public HealthRegenerationBonus(float bonus) => Value = bonus;
        public float Value { get; }
    }
}