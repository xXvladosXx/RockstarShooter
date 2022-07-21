using Characters.Player.Data.Colliders;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerGroundedState : PlayerMovementState
    {
        public PlayerGroundedState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.GroundedParameterHash);

            UpdateShouldSprintState();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.GroundedParameterHash);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            Float();
        }

        private void UpdateShouldSprintState()
        {
            if (!PlayerStateMachine.ReusableData.ShouldSprint)
            {
                return;
            }

            if (PlayerStateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            PlayerStateMachine.ReusableData.ShouldSprint = false;
        }

        private void Float()
        {
            Vector3 capsuleColliderCenterInWorldSpace = PlayerStateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleCenter = new Ray(capsuleColliderCenterInWorldSpace, Vector3.down);

            if (Physics.Raycast(downwardsRayFromCapsuleCenter, out RaycastHit hit, PlayerStateMachine.Player.ResizableCapsuleCollider.SlopeData.FloatRayDistance, PlayerStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                float groundAngle = Vector3.Angle(hit.normal, -downwardsRayFromCapsuleCenter.direction);

                float slopeSpeedModifier = SetSlopeSpeedModifierOnAngle(groundAngle);

                if (slopeSpeedModifier == 0f)
                {
                    return;
                }

                float distanceToFloatingPoint = PlayerStateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.ColliderCenterInLocalSpace.y * PlayerStateMachine.Player.transform.localScale.y - hit.distance;

                if (distanceToFloatingPoint == 0f)
                {
                    return;
                }

                float amountToLift = distanceToFloatingPoint * PlayerStateMachine.Player.ResizableCapsuleCollider.SlopeData.StepReachForce - GetPlayerVerticalVelocity().y;

                Vector3 liftForce = new Vector3(0f, amountToLift, 0f);

                PlayerStateMachine.Player.Rigidbody.AddForce(liftForce, ForceMode.VelocityChange);
            }
        }

        private float SetSlopeSpeedModifierOnAngle(float angle)
        {
            float slopeSpeedModifier = GroundedData.SlopeSpeedAngles.Evaluate(angle);

            if (PlayerStateMachine.ReusableData.MovementOnSlopesSpeedModifier != slopeSpeedModifier)
            {
                PlayerStateMachine.ReusableData.MovementOnSlopesSpeedModifier = slopeSpeedModifier;
            }

            return slopeSpeedModifier;
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerStateMachine.Player.Input.PlayerActions.Dash.started += OnDashStarted;

            PlayerStateMachine.Player.Input.PlayerActions.Jump.started += OnJumpStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerStateMachine.Player.Input.PlayerActions.Dash.started -= OnDashStarted;

            PlayerStateMachine.Player.Input.PlayerActions.Jump.started -= OnJumpStarted;
        }

        protected virtual void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.DashingState);
        }

        protected virtual void OnJumpStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.JumpingState);
        }

        protected virtual void OnMove()
        {
            if (PlayerStateMachine.ReusableData.ShouldSprint)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.SprintingState);

                return;
            }

            if (PlayerStateMachine.ReusableData.ShouldWalk)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }

        protected override void OnContactWithGroundExited(Collider collider)
        {
            if (IsThereGroundUnderneath())
            {
                return;
            }

            Vector3 capsuleColliderCenterInWorldSpace = PlayerStateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.Collider.bounds.center;

            Ray downwardsRayFromCapsuleBottom = new Ray(capsuleColliderCenterInWorldSpace - PlayerStateMachine.Player.ResizableCapsuleCollider.CapsuleColliderData.ColliderVerticalExtents, Vector3.down);

            if (!Physics.Raycast(downwardsRayFromCapsuleBottom, out _, GroundedData.GroundToFallRayDistance, PlayerStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore))
            {
                OnFall();
            }
        }

        private bool IsThereGroundUnderneath()
        {
            PlayerTriggerColliderData triggerColliderData = PlayerStateMachine.Player.ResizableCapsuleCollider.TriggerColliderData;

            Vector3 groundColliderCenterInWorldSpace = triggerColliderData.GroundCheckCollider.bounds.center;

            Collider[] overlappedGroundColliders = Physics.OverlapBox(groundColliderCenterInWorldSpace, triggerColliderData.GroundCheckColliderVerticalExtents, triggerColliderData.GroundCheckCollider.transform.rotation, PlayerStateMachine.Player.LayerData.GroundLayer, QueryTriggerInteraction.Ignore);

            return overlappedGroundColliders.Length > 0;
        }

        protected virtual void OnFall()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.FallingState);
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);

            UpdateTargetRotation(GetMovementInputDirection());
        }
    }
}