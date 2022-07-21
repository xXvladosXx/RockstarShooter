using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerWalkingFiringState : PlayerMovingState
    {
      
        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.WalkData.SpeedModifier;
            PlayerMovementStateMachine.ReusableData.MaxSmoothModifier = PlayerMovementStateMachine.Player.StateData.GroundedData.WalkData.SmoothInputSpeed;
            
            PlayerMovementStateMachine.Player.RigWeight = 1;

            base.Enter();
            
            PlayerMovementStateMachine.ReusableData.ShouldDecreaseAimLayer = false;
            
            ResetVelocity();

            PlayerMovementStateMachine.Player.Animator.SetLayerWeight(PlayerMovementStateMachine.Player.AnimationData.AimingLayer, 1);

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.WalkParameterHash);
            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.AimingParameterHash);

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
            
            return null;

        }

        public override void Update()
        {
            base.Update();

            SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.HorizontalParameterHash,
                GetMovementInputDirection().x, .1f);
            
            SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.VerticalParameterHash,
                GetMovementInputDirection().z, .1f);
            
            SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.SpeedParameterHash, 
                PlayerMovementStateMachine.ReusableData.SmoothModifier, 0.1f);

            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                LockToAim();

                PlayerMovementStateMachine.ReusableData.SmoothModifier -= Time.deltaTime;
            
                SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.SpeedParameterHash, 
                    PlayerMovementStateMachine.ReusableData.SmoothModifier, 0.1f);
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

            PlayerMovementStateMachine.Player.RigWeight = 0;
            
            PlayerMovementStateMachine.ReusableData.ShouldDecreaseAimLayer = true;
            PlayerMovementStateMachine.ReusableData.AimLayerWeight = 1;
            
            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.AimingParameterHash);
            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.WalkParameterHash);

            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                PlayerMovementStateMachine.ReusableData.SmoothModifier = 0;
                SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.SpeedParameterHash, 
                    PlayerMovementStateMachine.ReusableData.SmoothModifier);
            }
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            base.OnDashStarted(context);
            PlayerMovementStateMachine.ReusableData.AimLayerWeight = 0;
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            ResetVelocity();
        }
    }
}