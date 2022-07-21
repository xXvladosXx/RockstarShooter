using System;
using UnityEngine;

namespace Inventory.Core
{
    public abstract class Item : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public string Description { get; private set; }
        [field: SerializeField] public int MaxInStack { get; private set; }
        
        [field: SerializeField] public ItemData ItemData { get; private set; }
        [field: SerializeField] public Sprite Sprite { get; private set; }
        
        [field: SerializeField] public ItemType ItemType { get; private set; }
    }

    public enum ItemType
    {
        Weapon = 1,
        Rifle = Weapon | 1 << 2,
        Bullet = Weapon | 1 << 3,
        Grenade = 1 << 4,
        Aid = 1 << 5,
        SteelArms = Weapon | 1 << 6,
        
    }
}