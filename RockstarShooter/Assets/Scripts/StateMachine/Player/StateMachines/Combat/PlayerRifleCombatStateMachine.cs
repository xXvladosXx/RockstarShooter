using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.Data.States;
using Characters.Player.StateMachines.Movement;
using Characters.Player.StateMachines.Movement.States.Grounded;
using StateMachine.Player.StateMachines.Combat.Rifle;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerRifleCombatStateMachine : PlayerCombatStateMachine
    {
        public PlayerRifleCombatStateMachine(Player player) : base(player)
        {
            PlayerBaseIdleState = new PlayerRifleIdleState();
            PlayerBaseFireState = new PlayerRifleFiringState();
            PlayerBaseAimState = new PlayerRifleAimState();
            PlayerBaseReloadState = new PlayerRifleReloadState();
            PlayerBaseEquipState = new PlayerRifleEquipState();
            PlayerBaseDashState = new PlayerRifleDashingState();
        }

        
    }
}