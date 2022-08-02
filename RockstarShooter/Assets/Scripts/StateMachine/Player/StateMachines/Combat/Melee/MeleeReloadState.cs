using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;

namespace StateMachine.Player.StateMachines.Combat.Unarmed
{
    public class MeleeReloadState : PlayerBaseReloadState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 0;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;

            return null;
        }
        
    }
}