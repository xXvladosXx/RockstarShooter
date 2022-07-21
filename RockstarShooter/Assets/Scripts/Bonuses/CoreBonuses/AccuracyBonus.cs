namespace Bonuses.CoreBonuses
{
    public class AccuracyBonus : IBonus
    {
        public AccuracyBonus(float bonus) => Value = bonus;
        public float Value { get; }
    }
}