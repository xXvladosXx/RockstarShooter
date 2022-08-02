using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Rifle
{
    public class PlayerRifleReloadState : PlayerBaseReloadState
    {
        public override List<IState> Enter()
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldContinueAttack = false;
            return base.Enter();
        }
    }
}