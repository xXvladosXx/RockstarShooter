using Combat;
using Inventory.Core;
using Inventory.Items.Weapon;
using UnityEngine;

namespace Inventory
{
    public class ItemEquipper : MonoBehaviour
    {
        [field: SerializeField] public WeaponObject CurrentWeapon { get; private set; }
        [field: SerializeField] public Core.Inventory Inventory { get; private set; }
        [field: SerializeField] public Transform RightHand { get; private set; }
        
        public bool EquipWeapon(WeaponObject weaponObject)
        {
            if (CurrentWeapon != null) return false; 
            
            weaponObject = Instantiate(weaponObject, RightHand);
            
            Destroy(CurrentWeapon);
            CurrentWeapon = weaponObject;

            return true;
        }
        
        public void FireFromCurrentWeapon(AttackData attackData)
        {
            if(CurrentWeapon == null) return;
            
            CurrentWeapon.SpawnEmission(attackData);
        }

        public bool TryToEquipItem(Item pickableItemItem)
        {
            return pickableItemItem switch
            {
                WeaponItem weaponItem => EquipWeapon(weaponItem.WeaponObject),
                _ => false
            };
        }
    }
}