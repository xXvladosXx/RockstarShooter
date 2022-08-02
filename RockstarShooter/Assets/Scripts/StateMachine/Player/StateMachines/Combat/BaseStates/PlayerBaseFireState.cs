using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseFireState : PlayerBaseCombatState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.FiringLayer, 1);
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.AimingLayer, 1);
            
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseAimLayer = false;

            return new List<IState>()
            {
                new PlayerWalkingState()
            };
        }

        public override void Update()
        {
            base.Update();

            if (PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack)
            {
                PlayerCombatStateMachine.Player.AttackMaker.Fire();
            }
            
            SetSpeedAnimation(PlayerCombatStateMachine.Player.AnimationData.HorizontalParameterHash,
                GetMovementInputDirection().x, .1f);
            
            SetSpeedAnimation(PlayerCombatStateMachine.Player.AnimationData.VerticalParameterHash,
                GetMovementInputDirection().z, .1f);
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
            
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = true;
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseAimState);
        }

        public override void Exit()
        {
           base.Exit();
           
           PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.FiringLayer, 0);
            
           PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseAimLayer = true;
        }
    }
}