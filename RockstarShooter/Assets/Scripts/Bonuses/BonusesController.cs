using System;
using System.Collections.Generic;
using System.Linq;
using Bonuses.CoreBonuses;

namespace Bonuses
{
    public class BonusesController
    {
        private readonly List<IModifier> _modifiers;
        private Dictionary<IBonus, Stat> _currentBonuses;

        public BonusesController(List<IModifier> modifiers)
        {
            _modifiers = modifiers;
            _currentBonuses = new Dictionary<IBonus, Stat>();
        }

        public void AddBonus(IBonus bonus)
        {
            if(_currentBonuses.ContainsKey(bonus)) return;
            
            switch (bonus)
            {
                case AccuracyBonus accuracyBonus:
                    _currentBonuses.Add(accuracyBonus, Stat.Accuracy);
                    break;
                case AttackSpeedBonus attackSpeedBonus:
                    _currentBonuses.Add(attackSpeedBonus, Stat.AttackSpeed);
                    break;
                case CriticalChanceBonus criticalChanceBonus:
                    _currentBonuses.Add(criticalChanceBonus, Stat.CriticalChance);
                    break;
                case CriticalDamageBonus criticalDamageBonus:
                    _currentBonuses.Add(criticalDamageBonus, Stat.CriticalDamage);
                    break;
                case DamageBonus damageBonus:
                    _currentBonuses.Add(damageBonus, Stat.Damage);
                    break;
                case HealthBonus healthBonus:
                    _currentBonuses.Add(healthBonus, Stat.Health);
                    break;
                case HealthRegenerationBonus healthRegenerationBonus:
                    _currentBonuses.Add(healthRegenerationBonus, Stat.HealthRegeneration);
                    break;
                case ManaBonus manaBonus:
                    _currentBonuses.Add(manaBonus, Stat.Mana);
                    break;
                case ManaRegenerationBonus manaRegenerationBonus:
                    _currentBonuses.Add(manaRegenerationBonus, Stat.ManaRegeneration);
                    break;
                case MovementSpeedBonus movementSpeedBonus:
                    _currentBonuses.Add(movementSpeedBonus, Stat.MovementSpeed);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(bonus));
            }
        }

        public void RemoveBonus(IBonus bonus)
        {
            _currentBonuses.Remove(bonus);
        }

        public float GetStat(Stat stat)
        {
            var characteristicWithModifier = GetBonus(stat);
            float currentBonuses = _currentBonuses.Where(bonus => bonus.Value == stat).Sum(bonus => bonus.Key.Value);

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
                .SelectMany(x => x.AddBonus(new[] {stat}))
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