using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Unarmed
{
    public class MeleeIdleState : PlayerBaseIdleState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 0;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;

            return new List<IState>()
            {
                new PlayerIdlingState()
            };
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseAimState);
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseFireState);
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
        }
        
        
    }
}