using Inventory.Core;
using Inventory.Items.Bullet;
using UnityEditor.Animations;
using UnityEngine;

namespace Inventory.Items.Weapon
{
    [CreateAssetMenu (menuName = "InventorySystem/Item")]
    public class WeaponItem : EquipableItem
    {
        [field: SerializeField] public BulletObject BulletObject { get; private set; }
        [field: SerializeField] public WeaponObject WeaponObject { get; private set; }
        [field: SerializeField] public ParticleSystem[] HitParticles { get; private set; } 
        [field: SerializeField] public float MinSpread { get; private set; }
        [field: SerializeField] public float MaxSpread { get; private set; }
        [field: SerializeField] public float SpreadPerFire { get; private set; }
        [field: SerializeField] public int MaxAccuracyDebuff { get; private set; } = 15;
        [field: SerializeField] public float SpreadReduceModifier { get; private set; } = 2f;
        [field: SerializeField] public float TimeBetweenShooting { get; private set; } = 2f;
        [field: SerializeField] public float ReloadTime { get; private set; } = 2f;
        [field: SerializeField] public int MagazineSize { get; private set; } = 20;
        [field: SerializeField] public int BulletsPerTap { get; private set; } = 1;
        [field: SerializeField] public bool AllowButtonHold { get; private set; } = true;
        [field: SerializeField] public RuntimeAnimatorController AnimatorController { get; private set; }
    }
}