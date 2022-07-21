using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement.States.Airborne
{
    public class PlayerAirborneState : PlayerMovementState
    {
     

        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.AirborneParameterHash);

            ResetSprintState();
            
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.AirborneParameterHash);
        }

        protected virtual void ResetSprintState()
        {
            PlayerMovementStateMachine.ReusableData.ShouldSprint = false;
        }

        protected override void OnContactWithGround(Collider collider)
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.LightLandingState);
        }
    }
}