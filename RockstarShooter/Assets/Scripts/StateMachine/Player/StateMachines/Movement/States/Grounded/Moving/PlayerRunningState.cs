using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerRunningState : PlayerMovingState
    {
        private float _startTime;

        public PlayerRunningState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.RunData.SpeedModifier;

            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.RunParameterHash);
            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;

            _startTime = Time.time;
            
            PlayerStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(PlayerStateMachine.Player.AnimationData.RunParameterHash);
        }

        public override void Update()
        {
            base.Update();
            
            if (!PlayerStateMachine.ReusableData.ShouldWalk)
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
            if (PlayerStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerStateMachine.WalkingFiringState);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.MediumStoppingState);

            base.OnMovementCanceled(context);
        }
    }
}