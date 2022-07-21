using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerMediumStoppingState : PlayerStoppingState
    {
        public PlayerMediumStoppingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.MediumStopParameterHash);

            PlayerStateMachine.ReusableData.MovementDecelerationForce = GroundedData.StopData.MediumDecelerationForce;

            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.MediumStopParameterHash);
        }
    }
}