using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Player.Utilities.Colliders;
using Combat;
using Inventory.Core;
using Inventory.Items;
using Inventory.Items.Weapon;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Inventory
{
    public class ItemEquipper : MonoBehaviour
    {
        [field: SerializeField] public List<WeaponObject> WeaponObjects = new List<WeaponObject>();
        [field: SerializeField] public WeaponObject CurrentWeapon { get; private set; }
        [field: SerializeField] public WeaponObject PossibleWeapon { get; private set; }
        [field: SerializeField] public Core.Inventory Inventory { get; private set; }
        [field: SerializeField] public Transform RightHand { get; private set; }
        [field: SerializeField] public Transform LeftHand { get; private set; }

        private Dictionary<Item, ItemObject> _itemObjects = new Dictionary<Item, ItemObject>();
        
        private GameObject _currentMagazine;
        private int _weaponIndex;
        

        public void InitData(PlayerCameraSwitcher playerCameraSwitcher)
        {
            CurrentWeapon.Init(playerCameraSwitcher);
        }
        
        public bool EquipWeapon(WeaponObject weaponObject)
        {
            var weapon = Instantiate(weaponObject, RightHand);
            
            Destroy(CurrentWeapon.gameObject);
            CurrentWeapon = weapon;

            return true;
        }

        public bool TryToEquipItem(Item pickableItemItem, ItemObject itemObject)
        {
            _itemObjects.Add(pickableItemItem, itemObject);
            
            return pickableItemItem switch
            {
                WeaponItem weaponItem => EquipWeapon(weaponItem.WeaponObject),
                _ => false
            };
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                EquipWeapon(_itemObjects.Values.ElementAt(0) as WeaponObject);
            }
        }

        public void TakeMagazine()
        {
            _currentMagazine = Instantiate(CurrentWeapon.Magazine, LeftHand);
            CurrentWeapon.SetMagazine(_currentMagazine);
        }

        public void DropMagazine()
        {
            if (!CurrentWeapon.WasLoaded)
            {
                CurrentWeapon.DropMagazine();
            }
        }
        public void MagazineLoaded()
        {
            CurrentWeapon.LoadMagazine();
        }

        public void WeaponReloaded()
        {
            CurrentWeapon.ReloadFinished();
        }

        public void ChangeWeapon(Animator animator)
        {
            animator.runtimeAnimatorController = PossibleWeapon.WeaponItem.AnimatorController;
            EquipWeapon(PossibleWeapon);
        }

        public void ChangeWeaponIndex(int i)
        {
            _weaponIndex += i;
            if (_weaponIndex >= WeaponObjects.Count)
            {
                _weaponIndex--;
                return;
            }

            if (_weaponIndex < 0)
            {
                _weaponIndex++;
                return;
            }
            
            PossibleWeapon = WeaponObjects[_weaponIndex];
        }
    }
}