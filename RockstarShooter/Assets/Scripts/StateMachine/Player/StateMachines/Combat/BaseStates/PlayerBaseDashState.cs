using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseDashState : PlayerBaseIdleState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
            return null;
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(PlayerCombatStateMachine.Player.AnimationData.RunParameterHash);
        }
    }
}