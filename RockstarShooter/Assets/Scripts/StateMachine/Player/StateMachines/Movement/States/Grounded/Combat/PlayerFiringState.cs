/*using System;
using System.Collections;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerFiringState : PlayerWalkingFiringState
    {
        public PlayerFiringState(PlayerMovementStateMachine playerMovementPlayerMovementStateMachine) : base(playerMovementPlayerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerMovementStateMachine.ReusableData.ShouldDecreaseFireLayer = false;
            PlayerMovementStateMachine.ReusableData.ShouldFire = true;

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.FiringParameterHash);
            PlayerMovementStateMachine.Player.Animator.SetLayerWeight(PlayerMovementStateMachine.Player.AnimationData.FiringLayer, 1);
        }

      
        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerMovementStateMachine.ReusableData.ShouldFire = false;
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerMovementStateMachine.ReusableData.ShouldFire = false;
            base.OnDashStarted(context);
        }

        public override void Exit()
        {
            base.Exit();
            PlayerMovementStateMachine.ReusableData.ShouldDecreaseFireLayer = true;
            PlayerMovementStateMachine.ReusableData.FireLayerWeight = 0;
            PlayerMovementStateMachine.Player.Animator.SetLayerWeight(PlayerMovementStateMachine.Player.AnimationData.FiringLayer, 0);
            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.FiringParameterHash);
        }

    }
}*/