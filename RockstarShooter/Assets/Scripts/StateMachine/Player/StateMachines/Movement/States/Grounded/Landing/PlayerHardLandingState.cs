using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Landing
{
    public class PlayerHardLandingState : PlayerLandingState
    {


        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.HardLandParameterHash);

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.Disable();

            ResetVelocity();
            
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.HardLandParameterHash);

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.Enable();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (!IsMovingHorizontally())
            {
                return;
            }

            ResetVelocity();
        }

        public override void OnAnimationExitEvent()
        {
            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.Enable();
        }

        public override void OnAnimationTransitionEvent()
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;
        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }

        protected override void OnMove()
        {
            if (PlayerMovementStateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.RunningState);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
    }
}