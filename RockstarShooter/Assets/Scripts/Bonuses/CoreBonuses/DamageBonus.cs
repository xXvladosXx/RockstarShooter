using System;

namespace Bonuses.CoreBonuses
{
    [Serializable]
    public class DamageBonus : IBonus
    {
        public DamageBonus(float bonus) => Value = bonus;

        public float Value { get; }
    }
}