using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Unarmed
{
    public class MeleeAimState : PlayerBaseAimState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 0;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;

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

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;

            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseFireState);
        }
    }
}