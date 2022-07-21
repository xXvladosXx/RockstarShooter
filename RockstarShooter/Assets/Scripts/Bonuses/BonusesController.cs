using System.Collections.Generic;
using System.Linq;
using Bonuses.CoreBonuses;

namespace Bonuses
{
    public class BonusesController
    {
        private readonly List<IModifier> _modifiers;
        private Dictionary<IBonus,Stat> _currentBonuses;

        public BonusesController(List<IModifier> modifiers)
        {
            _modifiers = modifiers;
            _currentBonuses = new Dictionary<IBonus,Stat>();
        }

        public void AddBonus(IBonus bonus, Stat stat)
        {
            _currentBonuses.Add(bonus, stat);
        }

        public void RemoveBonus(IBonus bonus, Stat stat)
        {
            _currentBonuses.Remove(bonus, out stat);
        }

        public float GetStat(Stat stat)
        {
            var characteristicWithModifier = GetBonus(stat) ;
            float currentBonuses = _currentBonuses.Where(bonus => bonus.Value == stat).Sum(bonus => bonus.Key.Value);

            //float valueWithBonus = GetBonus(characteristics) + starterValue;
            
            return characteristicWithModifier + currentBonuses;
        }
        
        private float GetBonus(Stat stat)
        {
            bool IsBonusAssignableToCharacteristics(IBonus bonus) 
                => (bonus, stat) switch
                {
                    (HealthBonus b, Stat.Health) => true,
                    (DamageBonus b, Stat.Damage) => true,
                    (CriticalChanceBonus b, Stat.CriticalChance) => true,
                    (CriticalDamageBonus b, Stat.CriticalDamage) => true,
                    (AttackSpeedBonus b, Stat.AttackSpeed) => true,
                    (MovementSpeedBonus b, Stat.MovementSpeed) => true,
                    (ManaRegenerationBonus b, Stat.ManaRegeneration) => true,
                    (ManaBonus b, Stat.Mana) => true,
                    (HealthRegenerationBonus b, Stat.HealthRegeneration) => true,
                    (AccuracyBonus b, Stat.Accuracy) => true,
                    _ => false
                };

            return _modifiers
                .SelectMany(x => x.AddBonus(new[] { stat }))
                .Where(IsBonusAssignableToCharacteristics)
                .Sum(x => x.Value);
        }
    }


    public enum Stat
    {
        Health,
        HealthRegeneration,
        Mana,
        ManaRegeneration,
        Damage,
        CriticalChance,
        CriticalDamage,
        AttackSpeed,
        MovementSpeed,
        Accuracy
    }
    
    public enum Characteristic
    {
        Strength,
        Intelligence,
        Agility,
    }
}