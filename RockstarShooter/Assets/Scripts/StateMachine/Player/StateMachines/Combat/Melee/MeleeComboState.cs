using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Unarmed
{
    public class MeleeComboState : MeleeFireState
    {
        public override List<IState> Enter()
        {
            StartAnimation(PlayerCombatStateMachine.Player.AnimationData.ComboParameterHash);
            return base.Enter();
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            StartAnimation(PlayerCombatStateMachine.Player.AnimationData.ComboParameterHash);
        }

        public override void OnAnimationTransitionEvent()
        {
            StopAnimation(PlayerCombatStateMachine.Player.AnimationData.ComboParameterHash);
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(PlayerCombatStateMachine.Player.AnimationData.ComboParameterHash);
        }
    }
}