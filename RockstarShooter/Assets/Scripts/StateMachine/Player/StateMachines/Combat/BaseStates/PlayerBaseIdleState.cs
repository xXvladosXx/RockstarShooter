using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseIdleState : PlayerBaseCombatState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            return new List<IState>()
            {
                new PlayerIdlingState()
            };
        }

        public override void Update()
        {
            base.Update();
            if (PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack)
            {
                PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseFireState);
            }
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = true;

            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseFireState);
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseAimState);
        }

        protected override void OnEquipPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseEquipState);
        }
    }
}