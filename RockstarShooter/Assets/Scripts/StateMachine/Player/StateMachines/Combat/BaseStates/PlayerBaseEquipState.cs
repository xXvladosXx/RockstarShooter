using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseEquipState : PlayerBaseCombatState
    {
        public override List<IState> Enter()
        {
            if (PlayerCombatStateMachine.Player.ItemEquipper.CurrentWeapon.GetType() ==
                PlayerCombatStateMachine.Player.ItemEquipper.PossibleWeapon.GetType())
            {
                PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
                return null;
            }
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 0;
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.EquipLayerWeight = 1;
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseEquipLayer = false;

            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.EquippingLayer, 1);
            StartAnimation(PlayerCombatStateMachine.Player.AnimationData.EquippingParameterHash);

            return null;
        }

        public override void OnAnimationExitEvent()
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
        }

        public override void Exit()
        {
            base.Exit();

            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseEquipLayer = true;

            StopAnimation(PlayerCombatStateMachine.Player.AnimationData.EquippingParameterHash);
        }
    }
}