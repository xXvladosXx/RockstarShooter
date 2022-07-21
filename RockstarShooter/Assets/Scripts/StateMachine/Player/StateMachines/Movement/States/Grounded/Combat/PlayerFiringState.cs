using System;
using System.Collections;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerFiringState : PlayerWalkingFiringState
    {
        public PlayerFiringState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            
            PlayerStateMachine.ReusableData.ShouldDecreaseFireLayer = false;
            PlayerStateMachine.ReusableData.ShouldFire = true;

            StartAnimation(PlayerStateMachine.Player.AnimationData.FiringParameterHash);
            PlayerStateMachine.Player.Animator.SetLayerWeight(PlayerStateMachine.Player.AnimationData.FiringLayer, 1);
        }

      
        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ReusableData.ShouldFire = false;
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ReusableData.ShouldFire = false;
            base.OnDashStarted(context);
        }

        public override void Exit()
        {
            base.Exit();
            PlayerStateMachine.ReusableData.ShouldDecreaseFireLayer = true;
            PlayerStateMachine.ReusableData.FireLayerWeight = 0;
            PlayerStateMachine.Player.Animator.SetLayerWeight(PlayerStateMachine.Player.AnimationData.FiringLayer, 0);
            StopAnimation(PlayerStateMachine.Player.AnimationData.FiringParameterHash);
        }

    }
}