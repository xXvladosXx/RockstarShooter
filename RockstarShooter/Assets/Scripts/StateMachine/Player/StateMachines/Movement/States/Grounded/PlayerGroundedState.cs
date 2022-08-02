using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.Data.Colliders;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.GroundedParameterHash);

            UpdateShouldSprintState();

            return null;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.GroundedParameterHash);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Float();
        }

        private void UpdateShouldSprintState()
        {
            if (!PlayerMovementStateMachine.ReusableData.ShouldSprint)
            {
                return;
            }

            if (PlayerMovementStateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            PlayerMovementStateMachine.ReusableData.ShouldSprint = false;
        }

        protected void Float()
        {
            Vector3 capsuleColliderCenterInWorldSpace = PlayerMovementStateMachine.Player.ResizableCapsuleCollider
                .CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit,
                    PlayerMovementStateMachine.Player.ResizableCapsuleCollider.SlopeData.FloatRayDistance,
                    PlayerMovementStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                {
                    return;
                }

                float distanceToFloatingPoint =
                    PlayerMovementStateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData
                        .ColliderCenterInLocalSpace.y * PlayerMovementStateMachine.Player.transform.localScale.y -
                    hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift =
                    distanceToFloatingPoint * PlayerMovementStateMachine.Player.ResizableCapsuleCollider.SlopeData
                        .StepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                PlayerMovementStateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = GroundedData.SlopeSpeedAngles.Evaluate(angle);

            if (PlayerMovementStateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                PlayerMovementStateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
            }

            return slopeSpeedModifier;
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

            PlayerMovementStateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;

            PlayerMovementStateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerMovementStateMachine.DashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerMovementStateMachine.JumpingState);
        }

        protected virtual void OnMove()
        {
            if (PlayerMovementStateMachine.ReusableData.ShouldSprint)
            {
                PlayerStateMachine.ChangeState(PlayerMovementStateMachine.SprintingState);

                return;
            }

            if (PlayerMovementStateMachine.ReusableData.ShouldWalk)
            {
                PlayerStateMachine.ChangeState(PlayerMovementStateMachine.WalkingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerMovementStateMachine.RunningState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = PlayerMovementStateMachine.Player.ResizableCapsuleCollider
                .CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom =
                new Ray(
                    capsuleColliderCenterInWorldSpace - PlayerMovementStateMachine.Player.ResizableCapsuleCollider
                        .CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, GroundedData.GroundToFallRayDistance,
                    PlayerMovementStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        private bool IsThereGroundUnderneath()
        {
            PlayerTriggerColliderData triggerColliderData =
                PlayerMovementStateMachine.Player.ResizableCapsuleCollider.TriggerColliderData;

            Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace,
                triggerColliderData.GroundCheckColliderVerticalExtents,
                triggerColliderData.GroundCheckCollider.transform.rotation,
                PlayerMovementStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }

        protected virtual void OnFall()
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.FallingState);
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);

            UpdateTargetRotation(GetMovementInputDirection());
        }
    }
}