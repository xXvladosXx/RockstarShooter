using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;

namespace StateMachine.Player.StateMachines.Combat.Rifle
{
    public class PlayerRifleDashingState : PlayerBaseDashState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 1;

            return null;
        }
    }
}