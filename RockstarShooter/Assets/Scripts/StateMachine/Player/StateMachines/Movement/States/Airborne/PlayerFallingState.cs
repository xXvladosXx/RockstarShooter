using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement.States.Airborne
{
    public class PlayerFallingState : PlayerAirborneState
    {
        private Vector3 _playerPositionOnEnter;

        public PlayerFallingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.FallParameterHash);

            PlayerStateMachine.ReusableData.MovementSpeedModifier = 0f;

            _playerPositionOnEnter = PlayerStateMachine.Player.transform.position;

            ResetVerticalVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.FallParameterHash);
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

            PlayerStateMachine.Player.Rigidbody.AddForce(limitedVelocityForce, ForceMode.VelocityChange);
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnContactWithGround(Collider collider)
        {
            float fallDistance = _playerPositionOnEnter.y - PlayerStateMachine.Player.transform.position.y;

            if (fallDistance < AirborneData.FallData.MinimumDistanceToBeConsideredHardFall)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.LightLandingState);

                return;
            }

            if (PlayerStateMachine.ReusableData.ShouldWalk && !PlayerStateMachine.ReusableData.ShouldSprint || PlayerStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.HardLandingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.RollingState);

        }
    }
}