using System;
using Inventory.Items.Weapon;
using TMPro;
using UI.Core;
using UnityEngine;

namespace UI.Weapon
{
    public class AmmoUI : StaticUIElement
    {
        [SerializeField] private TextMeshProUGUI _text;
        private WeaponObject _weaponObject;
        public override void Init(UIData uiData)
        {
            base.Init(uiData);

            _weaponObject = uiData.Player.ItemEquipper.CurrentWeapon;
            _weaponObject.OnMagazineChanged += OnBulletChanged;
        }

        private void OnBulletChanged(int obj)
        {
            _text.text = $"{obj}/{_weaponObject.WeaponItem.MagazineSize}";
        }

        private void Update()
        {
            
        }
    }
}