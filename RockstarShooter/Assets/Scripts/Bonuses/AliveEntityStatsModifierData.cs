using System;
using RotaryHeart.Lib.SerializableDictionary;
using UnityEngine;

namespace Bonuses
{
    [CreateAssetMenu(menuName = "Stats/AliveEntityStatModifier")]
    public class AliveEntityStatsModifierData : ScriptableObject
    {
        [field: SerializeField] public StatModifierSerializableDictionary StatModifier { get; private set; }
    }

    [Serializable]
    public class StatModifierSerializableDictionary : SerializableDictionaryBase<Stat, StatCharacteristicModifier> { }

    [Serializable]
    public class StatCharacteristicModifier
    {
        public float Value;
        public Characteristic Characteristic;
    }
}