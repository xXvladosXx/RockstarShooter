using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement.States.Grounded
{
    public class PlayerIdlingState : PlayerGroundedState
    {
        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = 0f;

            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.IdleParameterHash);

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StationaryForce;

            ResetVelocity();
            
            return null;
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.IdleParameterHash);
        }

        public override void Update()
        {
            base.Update();
            
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero)
            {
                 return;
            }

            OnMove();
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
    }
}