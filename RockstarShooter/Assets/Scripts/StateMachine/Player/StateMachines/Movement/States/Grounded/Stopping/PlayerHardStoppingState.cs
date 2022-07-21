using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.HardStopParameterHash);

            PlayerMovementStateMachine.ReusableData.MovementDecelerationForce = GroundedData.StopData.HardDecelerationForce;

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;

            return null;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.HardStopParameterHash);
        }

        protected override void OnMove()
        {
            if (PlayerMovementStateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.RunningState);
        }
    }
}