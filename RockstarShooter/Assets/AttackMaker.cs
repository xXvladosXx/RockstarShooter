using System.Collections;
using System.Collections.Generic;
using Bonuses;
using Combat;
using Entity;
using Inventory;
using UnityEngine;
using UnityEngine.InputSystem;
using PlayerInput = Characters.Player.Utilities.Inputs.PlayerInput;

public class AttackMaker 
{
    public bool IsFiring { get; private set; }

    private PlayerInputActions _playerInputActions;
    private ItemEquipper _itemEquipper;
    private IDamageApplier _damageApplier;
    
    public AttackMaker(PlayerInputActions playerInputActions, ItemEquipper itemEquipper, IDamageApplier damageApplier)
    {
        _playerInputActions = playerInputActions;
        _itemEquipper = itemEquipper;
        _damageApplier = damageApplier;
    }

    
    
    public void Fire()
    {
       
            AttackData attackData = new AttackData
            {
                DamageApplier = _damageApplier,
                AccuracyModifier = _damageApplier.DamagerBonusesController().GetStat(Stat.Accuracy)
            };

            if (_itemEquipper.CurrentWeapon.CanMakeAttack())
            {
                _itemEquipper.CurrentWeapon.MakeAttack(attackData);
            }
    }
}
