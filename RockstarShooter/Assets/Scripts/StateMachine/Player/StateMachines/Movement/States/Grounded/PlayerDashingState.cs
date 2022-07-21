using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private bool _shouldKeepRotating;

        public PlayerDashingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.DashData.SpeedModifier;
            PlayerStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;

            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.DashParameterHash);

            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;

            PlayerStateMachine.ReusableData.RotationData = GroundedData.DashData.RotationData;

            _shouldKeepRotating = PlayerStateMachine.ReusableData.MovementInput != Vector2.zero;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.DashParameterHash);

            SetBaseRotationData();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!_shouldKeepRotating)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationEnterEvent()
        {
            base.OnAnimationEnterEvent();
            Dash();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (PlayerStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.HardStoppingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.SprintingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
            PlayerStateMachine.Player.Input.PlayerActions.Dash.performed -= OnDashStarted;

        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);

            _shouldKeepRotating = true;
        }

        private void Dash()
        {
            Vector3 dashDirection = PlayerStateMachine.Player.transform.forward;

            dashDirection.y = 0f;

            UpdateTargetRotation(dashDirection, false);

            if (PlayerStateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementInputDirection());

                dashDirection = GetTargetRotationDirection(PlayerStateMachine.ReusableData.CurrentTargetRotation.y);
            }

            PlayerStateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
        }
    }
}