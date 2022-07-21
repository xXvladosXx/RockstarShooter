using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement.States.Airborne
{
    public class PlayerAirborneState : PlayerMovementState
    {
        public PlayerAirborneState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.AirborneParameterHash);

            ResetSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.AirborneParameterHash);
        }

        protected virtual void ResetSprintState()
        {
            PlayerStateMachine.ReusableData.ShouldSprint = false;
        }

        protected override void OnContactWithGround(Collider collider)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.LightLandingState);
        }
    }
}