using Inventory.Core;
using Inventory.Items.Bullet;
using UnityEngine;

namespace Inventory.Items.Weapon
{
    [CreateAssetMenu (menuName = "InventorySystem/Item")]
    public class WeaponItem : EquipableItem
    {
        [field: SerializeField] public BulletObject BulletObject { get; private set; }
        [field: SerializeField] public WeaponObject WeaponObject { get; private set; }
        [field: SerializeField] public float TimeToReduceSpread { get; private set; } = 1f;
        [field: SerializeField] public float MinSpread { get; private set; }
        [field: SerializeField] public float MaxSpread { get; private set; }
        [field: SerializeField] public float SpreadPerFire { get; private set; }

        [field: SerializeField] public float ShootForce { get; private set; }
        [field: SerializeField] public float SpreadReduceModifier { get; private set; } = 2f;
    }
}