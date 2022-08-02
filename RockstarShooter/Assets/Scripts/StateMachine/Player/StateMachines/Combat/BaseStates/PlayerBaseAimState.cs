using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseAimState : PlayerBaseFireState
    {

        public override List<IState> Enter()
        {
            PlayerCombatStateMachine.Player.PlayerCameraSwitcher.SwitchToAimCamera();

            base.Enter();
            
            if (!PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack)
            {
                PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.FiringLayer, 0);
            }
            
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseAimLayer = false;
            
            return new List<IState>()
            {
                new PlayerWalkingState()
            };
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
        }
        
        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
        }

        public override void Exit()
        {
            base.Exit();

            PlayerCombatStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
        }
    }
}