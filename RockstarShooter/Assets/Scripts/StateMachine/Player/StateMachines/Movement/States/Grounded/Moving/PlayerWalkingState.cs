using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerWalkingState : PlayerMovingState
    {
        private IBonus _accuracyDebuff = new AccuracyBonus(-75);
        public override List<IState> Enter()
        {
            if (GetMovementInputDirection() != Vector3.zero)
            {
                AddMovementDebuff(_accuracyDebuff);
            }
            
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.WalkData.SpeedModifier;
            PlayerMovementStateMachine.ReusableData.MaxSmoothModifier = PlayerMovementStateMachine.Player.StateData.GroundedData.WalkData.SmoothInputSpeed;
            
            base.Enter();
            
            ResetVelocity();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.WalkParameterHash);
            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.AimingParameterHash);

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.WeakForce;
            
            return null;
        }
        

        public override void Update()
        {
            base.Update();

            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                LockToAim();
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

            RemoveMovementDebuff(_accuracyDebuff);
           
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

        protected override void OnMovementPerformed(InputAction.CallbackContext context)
        {
            base.OnMovementPerformed(context);
            AddMovementDebuff(_accuracyDebuff);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            ResetVelocity();
            RemoveMovementDebuff(_accuracyDebuff);
        }
        
    }
}