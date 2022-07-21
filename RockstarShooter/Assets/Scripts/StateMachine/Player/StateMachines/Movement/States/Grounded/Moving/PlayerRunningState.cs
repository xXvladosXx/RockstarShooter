using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerRunningState : PlayerMovingState
    {
        private float _startTime;



        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.RunData.SpeedModifier;

            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.RunParameterHash);
            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;

            _startTime = Time.time;
            
            PlayerMovementStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;
            return null;

        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.RunParameterHash);
        }

        public override void Update()
        {
            base.Update();
            
            if (!PlayerMovementStateMachine.ReusableData.ShouldWalk)
            {
                return;
            }

            if (Time.time < _startTime + GroundedData.SprintData.RunToWalkTime)
            {
                return;
            }

            StopRunning();
        }

        private void StopRunning()
        {
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);

                return;
            }

            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.WalkingFiringState);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.MediumStoppingState);

            base.OnMovementCanceled(context);
        }
    }
}