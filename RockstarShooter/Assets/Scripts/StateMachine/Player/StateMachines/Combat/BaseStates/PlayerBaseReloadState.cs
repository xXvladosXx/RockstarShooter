using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseReloadState : PlayerBaseFireState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.FiringLayer, 0);
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.ReloadLayer, 1);
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 0;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;
            
            StartAnimation(PlayerCombatStateMachine.Player.AnimationData.ReloadParameterHash);
            
             return new List<IState>()
            {
                new PlayerWalkingState()
            };
        }
        
        public override void OnAnimationTransitionEvent()
        {
            base.OnAnimationExitEvent();

            PlayerCombatStateMachine.Player.LeftRigWeight = 1;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 1;
        }
        public override void OnAnimationExitEvent()
        {
            base.OnAnimationExitEvent();

            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = true;
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 1;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;
            
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.ReloadLayer, 0);

            PlayerCombatStateMachine.Player.OnMagazineDrop();
            
            StopAnimation(PlayerCombatStateMachine.Player.AnimationData.ReloadParameterHash);
        }
    }
}