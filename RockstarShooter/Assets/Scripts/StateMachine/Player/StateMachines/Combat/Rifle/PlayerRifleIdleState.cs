using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Rifle.Firing
{
    public class PlayerRifleIdleState : PlayerBaseIdleState
    {
        public override List<IState> Enter()
        {
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;
            PlayerCombatStateMachine.Player.LeftRigWeight = 1;
            
            return base.Enter();
        }
    }
}