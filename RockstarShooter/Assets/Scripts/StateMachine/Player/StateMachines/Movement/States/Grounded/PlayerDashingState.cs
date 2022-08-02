using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using StateMachine.Player.StateMachines.Combat;
using StateMachine.Player.StateMachines.Combat.Rifle;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerDashingState : PlayerGroundedState
    {
        private bool _shouldKeepRotating;

        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.DashData.SpeedModifier;
            PlayerMovementStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;

            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.DashParameterHash);

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;

            PlayerMovementStateMachine.ReusableData.RotationData = GroundedData.DashData.RotationData;
            
            PlayerMovementStateMachine.ReusableData.SmoothModifier =
                PlayerMovementStateMachine.ReusableData.MaxSmoothModifier;
            
            _shouldKeepRotating = PlayerMovementStateMachine.ReusableData.MovementInput != Vector2.zero;

            return new List<IState>()
            {
                new PlayerBaseDashState()
            };
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.DashParameterHash);

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
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerStateMachine.ChangeState(PlayerMovementStateMachine.HardStoppingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerMovementStateMachine.SprintingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
            PlayerMovementStateMachine.Player.Input.PlayerActions.Dash.performed -= OnDashStarted;

        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
        }

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);

            _shouldKeepRotating = true;
        }

        private void Dash()
        {
            Vector3 dashDirection = PlayerMovementStateMachine.Player.transform.forward;

            dashDirection.y = 0f;

            UpdateTargetRotation(dashDirection, false);

            if (PlayerMovementStateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                UpdateTargetRotation(GetMovementInputDirection());

                dashDirection = GetTargetRotationDirection(PlayerMovementStateMachine.ReusableData.CurrentTargetRotation.y);
            }

            PlayerMovementStateMachine.Player.Rigidbody.velocity = dashDirection * GetMovementSpeed(false);
        }
    }
}