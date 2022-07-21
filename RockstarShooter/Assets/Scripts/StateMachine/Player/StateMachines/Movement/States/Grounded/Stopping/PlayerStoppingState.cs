using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Stopping
{
    public class PlayerStoppingState : PlayerGroundedState
    {
        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = 0f;
            PlayerMovementStateMachine.ReusableData.SmoothModifier = 0;
            
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.StoppingParameterHash);

            return null;
        }

        public override void Exit()
        {
            base.Exit();

            SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.SpeedParameterHash, 0);

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.StoppingParameterHash);
        }

        public override void Update()
        {
            base.Update();
            PlayerMovementStateMachine.ReusableData.SmoothModifier -= Time.deltaTime;
            
            SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.SpeedParameterHash, PlayerMovementStateMachine.ReusableData.SmoothModifier, 0.1f);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            RotateTowardsTargetRotation();

            if (!IsMovingHorizontally())
            {
                return;
            }

            DecelerateHorizontally();
        }

        public override void OnAnimationExitEvent()
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
    }
}