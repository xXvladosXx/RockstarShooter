using System;
using Characters.Player.Utilities.Colliders;
using Combat;

namespace Inventory.Items.Weapon
{
    public class MeleeWeaponObject : WeaponObject
    {
        public override event Action OnReload;
        public override event Action<int> OnMagazineChanged;

        public override void Init(PlayerCameraSwitcher playerCameraSwitcher)
        {
            
        }

        public override bool CanMakeAttack()
        {
            return true;
        }

        public override void MakeAttack(AttackData attackData)
        {
            
        }
    }
}