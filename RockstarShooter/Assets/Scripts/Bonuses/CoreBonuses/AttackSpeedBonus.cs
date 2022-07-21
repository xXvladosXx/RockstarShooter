namespace Bonuses.CoreBonuses
{
    public class AttackSpeedBonus : IBonus
    {
        public AttackSpeedBonus(float speed) => Value = speed;
        public float Value { get; }
    }
}