using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
      
        public override List<IState> Enter()
        {
            base.Enter();

            PlayerMovementStateMachine.ReusableData.MovementDecelerationForce = GroundedData.StopData.LightDecelerationForce;

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
            return null;

        }
    }
}