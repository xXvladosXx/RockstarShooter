using System;
using Bonuses;
using Inventory.Items.Weapon;
using UI.Core;
using UnityEngine;

namespace UI.Crosshair
{
    public class CrosshairUI : StaticUIElement
    {
        [SerializeField] private float _speed;
        [SerializeField] private float _minSize = 10;
        [SerializeField] private float _maxSize = 100;
        [SerializeField] private float _accuracyModifier = 50;
        
        private RectTransform _crosshair;

        private float _startHeight;
        private float _startSize;
        public WeaponObject WeaponObject { get; private set; }

        private float _currentSpread;
        private float _currentSize;
        private BonusesController _bonusesController;

        public override void Init(UIData uiData)
        {
            base.Init(uiData);

            WeaponObject = uiData.Player.ItemEquipper.CurrentWeapon;

            _crosshair = GetComponent<RectTransform>();
            _bonusesController = uiData.Player.BonusesController;

            _startSize = _crosshair.sizeDelta.x;
            _currentSize = _startSize;
        }

        private void Update()
        {
            if (WeaponObject.AimSpread != 0)
            {
                _currentSize = Mathf.Lerp(_currentSize, (WeaponObject.AimSpread + _startSize)/
                                                        (_bonusesController.GetStat(Stat.Accuracy)/_accuracyModifier),
                                                        Time.deltaTime * _speed);
            }

            _currentSize = Mathf.Clamp(_currentSize, _minSize, _maxSize);
            _crosshair.sizeDelta = new Vector2(_currentSize, _currentSize);
        }
    }
}