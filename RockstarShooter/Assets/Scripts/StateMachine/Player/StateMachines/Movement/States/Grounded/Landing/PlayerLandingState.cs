using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Landing
{
    public class PlayerLandingState : PlayerGroundedState
    {

        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.LandingParameterHash);
            
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.LandingParameterHash);
        }
    }
}