using GenshinImpactMovementSystem;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Landing
{
    public class PlayerHardLandingState : PlayerLandingState
    {
        public PlayerHardLandingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.HardLandParameterHash);

            PlayerStateMachine.Player.Input.PlayerActions.Movement.Disable();

            ResetVelocity();
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.HardLandParameterHash);

            PlayerStateMachine.Player.Input.PlayerActions.Movement.Enable();
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
            PlayerStateMachine.Player.Input.PlayerActions.Movement.Enable();
        }

        public override void OnAnimationTransitionEvent()
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();

            PlayerStateMachine.Player.Input.PlayerActions.Movement.started += OnMovementStarted;
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();

            PlayerStateMachine.Player.Input.PlayerActions.Movement.started -= OnMovementStarted;
        }

        private void OnMovementStarted(InputAction.CallbackContext context)
        {
            OnMove();
        }

        protected override void OnMove()
        {
            if (PlayerStateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
    }
}