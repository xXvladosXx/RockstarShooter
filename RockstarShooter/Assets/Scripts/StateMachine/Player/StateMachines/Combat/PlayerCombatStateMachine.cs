using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerCombatStateMachine : StateMachine<ICombatState>
    {
        public Player Player { get; }
        
        public PlayerCombatStateMachine(Player player)
        {
            Player = player;
        }

        public override List<IState> ChangeState(ICombatState newState)
        {
            newState.SetData(this);
            return base.ChangeState(newState);
        }
    }
}