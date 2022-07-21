using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerWalkingFiringState : PlayerMovingState
    {
        public PlayerWalkingFiringState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            PlayerStateMachine.ReusableData.MovementSpeedModifier = GroundedData.WalkData.SpeedModifier;
            PlayerStateMachine.ReusableData.MaxSmoothModifier = PlayerStateMachine.Player.StateData.GroundedData.WalkData.SmoothInputSpeed;
            
            PlayerStateMachine.Player.RigWeight = 1;

            base.Enter();
            
            PlayerStateMachine.ReusableData.ShouldDecreaseAimLayer = false;
            
            ResetVelocity();

            PlayerStateMachine.Player.Animator.SetLayerWeight(PlayerStateMachine.Player.AnimationData.AimingLayer, 1);

            StartAnimation(PlayerStateMachine.Player.AnimationData.WalkParameterHash);
            StartAnimation(PlayerStateMachine.Player.AnimationData.AimingParameterHash);

            PlayerStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
        }

        public override void Update()
        {
            base.Update();

            SetSpeedAnimation(PlayerStateMachine.Player.AnimationData.HorizontalParameterHash,
                GetMovementInputDirection().x, .1f);
            
            SetSpeedAnimation(PlayerStateMachine.Player.AnimationData.VerticalParameterHash,
                GetMovementInputDirection().z, .1f);
            
            SetSpeedAnimation(PlayerStateMachine.Player.AnimationData.SpeedParameterHash, 
                PlayerStateMachine.ReusableData.SmoothModifier, 0.1f);

            if (PlayerStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                LockToAim();

                PlayerStateMachine.ReusableData.SmoothModifier -= Time.deltaTime;
            
                SetSpeedAnimation(PlayerStateMachine.Player.AnimationData.SpeedParameterHash, 
                    PlayerStateMachine.ReusableData.SmoothModifier, 0.1f);
            }
        }

        public override void FixedUpdate()
        {
            MoveWithAimLocked();
            Float();
        }

        public override void Exit()
        {
            base.Exit();

            PlayerStateMachine.Player.RigWeight = 0;
            
            PlayerStateMachine.ReusableData.ShouldDecreaseAimLayer = true;
            PlayerStateMachine.ReusableData.AimLayerWeight = 1;
            
            StopAnimation(PlayerStateMachine.Player.AnimationData.AimingParameterHash);
            StopAnimation(PlayerStateMachine.Player.AnimationData.WalkParameterHash);

            if (PlayerStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerStateMachine.ReusableData.SmoothModifier = 0;
                SetSpeedAnimation(PlayerStateMachine.Player.AnimationData.SpeedParameterHash, 
                    PlayerStateMachine.ReusableData.SmoothModifier);
            }
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            base.OnDashStarted(context);
            PlayerStateMachine.ReusableData.AimLayerWeight = 0;
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            ResetVelocity();
        }
    }
}