using GenshinImpactMovementSystem;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerWalkingState : PlayerMovingState
    {
        public PlayerWalkingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.WalkData.SpeedModifier;

            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.WalkParameterHash);

            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.WalkParameterHash);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);

            PlayerStateMachine.ChangeState(PlayerStateMachine.RunningState);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);

            base.OnMovementCanceled(context);
        }
    }
}