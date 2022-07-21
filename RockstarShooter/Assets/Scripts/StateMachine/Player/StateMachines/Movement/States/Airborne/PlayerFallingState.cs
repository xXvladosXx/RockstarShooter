using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement.States.Airborne
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private Vector3 _playerPositionOnEnter;

      
        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.FallParameterHash);

            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = 0f;

            _playerPositionOnEnter = PlayerMovementStateMachine.Player.transform.position;

            ResetVerticalVelocity();
            
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.FallParameterHash);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            LimitVerticalVelocity();
        }

        private void LimitVerticalVelocity()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            if (playerVerticalVelocity.y >= -AirborneData.FallData.FallSpeedLimit)
            {
                return;
            }

            Vector3 limitedVelocityForce = new Vector3(0f, -AirborneData.FallData.FallSpeedLimit - playerVerticalVelocity.y, 0f);

            PlayerMovementStateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = _playerPositionOnEnter.y - PlayerMovementStateMachine.Player.transform.position.y;

            if (fallDistance < AirborneData.FallData.MinimumDistanceToBeConsideredHardFall)
            {
                PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.LightLandingState);

                return;
            }

            if (PlayerMovementStateMachine.ReusableData.ShouldWalk && !PlayerMovementStateMachine.ReusableData.ShouldSprint || PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.HardLandingState);

                return;
            }

            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.RollingState);

        }
    }
}