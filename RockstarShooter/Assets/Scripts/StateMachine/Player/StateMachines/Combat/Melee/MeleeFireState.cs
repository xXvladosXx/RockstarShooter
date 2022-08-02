using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using StateMachine.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Unarmed
{
    public class MeleeFireState : PlayerBaseFireState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.LeftRigWeight = 0;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;
            PlayerCombatStateMachine.Player.Animator.applyRootMotion = true;
            
            StartAnimation(PlayerCombatStateMachine.Player.AnimationData.FiringParameterHash);
            
            return new List<IState>()
            {
                new PlayerUnmovableState()
            };
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseComboState);
        }

        protected override void OnReloadPerformed()
        {
        }

        public override void OnAnimationEnterEvent()
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;

        }

        public override void Exit()
        {
            base.Exit();
            
            PlayerCombatStateMachine.Player.Animator.applyRootMotion = false;
            StopAnimation(PlayerCombatStateMachine.Player.AnimationData.FiringParameterHash);
        }
    }
}