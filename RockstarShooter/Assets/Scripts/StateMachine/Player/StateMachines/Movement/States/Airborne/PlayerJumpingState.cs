using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Airborne
{
    public class PlayerJumpingState : PlayerAirborneState
    {
        private bool _shouldKeepRotating;
        private bool _canStartFalling;

       
        public override List<IState> Enter()
        {
            base.Enter();

            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = 0f;

            PlayerMovementStateMachine.ReusableData.MovementDecelerationForce = AirborneData.JumpData.DecelerationForce;

            PlayerMovementStateMachine.ReusableData.RotationData = AirborneData.JumpData.RotationData;

            _shouldKeepRotating = PlayerMovementStateMachine.ReusableData.MovementInput != Vector2.zero;

            Jump();
            
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            SetBaseRotationData();

            _canStartFalling = false;
        }

        public override void Update()
        {
            base.Update();

            if (!_canStartFalling && IsMovingUp(0f))
            {
                _canStartFalling = true;
            }

            if (!_canStartFalling || IsMovingUp(0f))
            {
                return;
            }

            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.FallingState);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (_shouldKeepRotating)
            {
                RotateTowardsTargetRotation();
            }

            if (IsMovingUp())
            {
                DecelerateVertically();
            }
        }

        private void Jump()
        {
            Vector3 jumpForce = PlayerMovementStateMachine.ReusableData.CurrentJumpForce;

            Vector3 jumpDirection = PlayerMovementStateMachine.Player.transform.forward;

            if (_shouldKeepRotating)
            {
                UpdateTargetRotation(GetMovementInputDirection());

                jumpDirection = GetTargetRotationDirection(PlayerMovementStateMachine.ReusableData.CurrentTargetRotation.y);
            }

            jumpForce.x *= jumpDirection.x;
            jumpForce.z *= jumpDirection.z;

            jumpForce = GetJumpForceOnSlope(jumpForce);

            ResetVelocity();

            PlayerMovementStateMachine.Player.Rigidbody.AddForce(jumpForce, ForceMode.VelocityChange);
        }

        private Vector3 GetJumpForceOnSlope(Vector3 jumpForce)
        {
            Vector3 capsuleColliderCenterInWorldSpace = PlayerMovementStateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, AirborneData.JumpData.JumpToGroundRayDistance, PlayerMovementStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                if (IsMovingUp())
                {
                    float forceModifier = AirborneData.JumpData.JumpForceModifierOnSlopeUpwards.Evaluate(groundAngle);

                    jumpForce.x *= forceModifier;
                    jumpForce.z *= forceModifier;
                }

                if (IsMovingDown())
                {
                    float forceModifier = AirborneData.JumpData.JumpForceModifierOnSlopeDownwards.Evaluate(groundAngle);

                    jumpForce.y *= forceModifier;
                }
            }

            return jumpForce;
        }

        protected override void ResetSprintState()
        {
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
    }
}