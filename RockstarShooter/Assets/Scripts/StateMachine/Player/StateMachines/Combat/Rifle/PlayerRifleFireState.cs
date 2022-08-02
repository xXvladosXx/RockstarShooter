using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement;
using Characters.Player.StateMachines.Movement.States.Grounded;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat.Rifle.Firing
{
    public class PlayerRifleFiringState : PlayerBaseFireState
    {
        public override List<IState> Enter()
        {
            base.Enter();
            
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 1;
            PlayerCombatStateMachine.Player.LeftRigWeight = 1;

            return new List<IState>
            {
                new PlayerWalkingState()
            };
        }

        public override void Exit()
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.AimLayerWeight = 1;
            PlayerCombatStateMachine.Player.RightHandBodyRigWeight = 0;
            
            base.Exit();
        }
    }
}