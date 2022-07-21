using System;

namespace Bonuses.CoreBonuses
{
    [Serializable]
    public class CriticalChanceBonus : IBonus
    {
        public CriticalChanceBonus(float bonus) => Value = bonus;
        public float Value { get; }
    }
}