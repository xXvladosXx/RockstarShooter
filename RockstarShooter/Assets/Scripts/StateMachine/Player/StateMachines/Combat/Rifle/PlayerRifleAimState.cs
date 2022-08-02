using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Rifle.Firing
{
    
    public class PlayerRifleAimState : PlayerBaseAimState
    {
        private readonly IBonus _accuracyBonus = new AccuracyBonus(25);

        public override List<IState> Enter()
        {
            PlayerStateMachine.Player.BonusesController.AddBonus(_accuracyBonus);
            
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 1;

            return base.Enter();
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            base.OnFirePerformed(obj);
            
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.FiringLayer, 1);
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            base.OnFireCanceled(obj);
            
            PlayerCombatStateMachine.Player.Animator.SetLayerWeight(PlayerCombatStateMachine.Player.AnimationData.FiringLayer, 0);
        }

        public override void Exit()
        {
            PlayerStateMachine.Player.BonusesController.RemoveBonus(_accuracyBonus);

            base.Exit();
        }
    }
}