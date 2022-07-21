using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerHardStoppingState : PlayerStoppingState
    {
        public PlayerHardStoppingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.HardStopParameterHash);

            PlayerStateMachine.ReusableData.MovementDecelerationForce = GroundedData.StopData.HardDecelerationForce;

            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.HardStopParameterHash);
        }

        protected override void OnMove()
        {
            if (PlayerStateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }
    }
}