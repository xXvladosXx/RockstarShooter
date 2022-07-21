using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Landing
{
    public class PlayerLandingState : PlayerGroundedState
    {
        public PlayerLandingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.LandingParameterHash);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.LandingParameterHash);
        }
    }
}