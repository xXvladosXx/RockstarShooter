using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Landing
{
    public class PlayerRollingState : PlayerLandingState
    {

        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.RollData.SpeedModifier;

            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.RollParameterHash);

            PlayerMovementStateMachine.ReusableData.ShouldSprint = false;
            return null;

        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.RollParameterHash);
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();

            if (PlayerMovementStateMachine.ReusableData.MovementInput != Vector2.zero)
            {
                return;
            }

            RotateTowardsTargetRotation();
        }

        public override void OnAnimationTransitionEvent()
        {
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.MediumStoppingState);

                return;
            }

            OnMove();
        }

        protected override void OnJumpStarted(InputAction.CallbackContext context)
        {
        }
    }
}