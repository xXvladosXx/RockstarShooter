using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerLightStoppingState : PlayerStoppingState
    {
        public PlayerLightStoppingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            PlayerStateMachine.ReusableData.MovementDecelerationForce = GroundedData.StopData.LightDecelerationForce;

            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }
    }
}