using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {

        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.MediumStopParameterHash);

            PlayerMovementStateMachine.ReusableData.MovementDecelerationForce = GroundedData.StopData.MediumDecelerationForce;

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.MediumStopParameterHash);
        }
    }
}