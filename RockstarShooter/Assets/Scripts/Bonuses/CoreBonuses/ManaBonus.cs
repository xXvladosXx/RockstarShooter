namespace Bonuses.CoreBonuses
{
    public class ManaBonus : IBonus
    {
        public ManaBonus(float bonus) => Value = bonus;

        public float Value { get; }
    }
}