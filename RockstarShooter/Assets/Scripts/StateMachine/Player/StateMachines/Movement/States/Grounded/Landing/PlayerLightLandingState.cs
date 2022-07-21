using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Landing
{
    public class PlayerLightLandingState : PlayerLandingState
    {

        public override List<IState> Enter()
        {
            PlayerMovementStateMachine.ReusableData.MovementSpeedModifier = 0;

            base.Enter();

            PlayerMovementStateMachine.ReusableData.CurrentJumpForce = AirborneData.JumpData.StationaryForce;

            ResetVelocity();
            
            return null;

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

        public override void OnAnimationTransitionEvent()
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
        }
    }
}