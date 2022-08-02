using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Bonuses;
using Bonuses.CoreBonuses;
using GenshinImpactMovementSystem;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerRunningState : PlayerMovingState
    {
        private float _startTime;

        private IBonus _accuracyDebuff = new AccuracyBonus(-75);

        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = GroundedData.RunData.SpeedModifier;

            base.Enter();

            AddMovementDebuff(_accuracyDebuff);
            
            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.RunParameterHash);
            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.MediumForce;

            _startTime = Time.time;
            
            PlayerMovementStateMachine.ReusableData.MaxSmoothModifier = GroundedData.RunData.SmoothInputSpeed;

            return null;
        }

        public override void Exit()
        {
            base.Exit();
            
            RemoveMovementDebuff(_accuracyDebuff);
            
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
                PlayerStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);

                return;
            }

            PlayerStateMachine.ChangeState(PlayerMovementStateMachine.WalkingState);
        }

        protected override void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            base.OnWalkToggleStarted(context);
        }

        protected override void OnMovementCanceled(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ChangeState(PlayerMovementStateMachine.MediumStoppingState);

            base.OnMovementCanceled(context);
        }
    }
}