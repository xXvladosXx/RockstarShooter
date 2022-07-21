using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerSprintingState : PlayerMovingState
    {
        private float _startTime;

        private bool _keepSprinting;
        private bool _shouldResetSprintState;

 
        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.SprintData.SpeedModifier;

            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.SprintParameterHash);

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StrongForce;

            _startTime = Time.time;

            _shouldResetSprintState = true;

            if (!PlayerMovementStateMachine.ReusableData.ShouldSprint)
            {
                _keepSprinting = false;
            }
            
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.SprintParameterHash);

            if (_shouldResetSprintState)
            {
                _keepSprinting = false;

                PlayerMovementStateMachine.ReusableData.ShouldSprint = false;
            }
        }

        public override void Update()
        {
            base.Update();

            if (_keepSprinting)
            {
                return;
            }

            if (Time.time < _startTime + GroundedData.SprintData.SprintToRunTime)
            {
                return;
            }

            StopSprinting();
        }

        private void StopSprinting()
        {
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);

                return;
            }

            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.RunningState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Sprint.performed += OnSprintPerformed;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Sprint.performed -= OnSprintPerformed;
        }

        private void OnSprintPerformed(InputAction.CallbackContext context)
        {
            _keepSprinting = true;

            PlayerMovementStateMachine.ReusableData.ShouldSprint = true;
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.HardStoppingState);

            base.OnMovementCanceled(context);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
            _shouldResetSprintState = false;

            base.OnJumpStarted(context);
        }

        protected override void OnFall()
        {
            _shouldResetSprintState = false;

            base.OnFall();
        }
    }
}