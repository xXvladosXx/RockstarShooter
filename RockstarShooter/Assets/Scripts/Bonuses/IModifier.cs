using System;
using System.Collections.Generic;

namespace Bonuses
{
    public interface IModifier
    {
        public event Action OnStatModified;
        IEnumerable<IBonus> AddBonus(Stat[] stats);
    }
}