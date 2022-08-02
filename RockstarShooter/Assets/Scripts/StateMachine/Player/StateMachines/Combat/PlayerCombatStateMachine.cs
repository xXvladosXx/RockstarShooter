using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.Data.States;
using StateMachine.Player.StateMachines.Combat.Rifle.Firing;
using UnityEngine;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerCombatStateMachine : StateMachine<ICombatState>
    {
        public Player Player { get; }
        
        public AliveEntityCombatStateReusableData AliveEntityCombatStateReusableData { get; }
        
        public PlayerBaseIdleState PlayerBaseIdleState { get; protected set; }
        public PlayerBaseAimState PlayerBaseAimState { get; protected set; }
        public PlayerBaseFireState PlayerBaseFireState { get; protected set; }
        public PlayerBaseDashState PlayerBaseDashState { get; protected set; }
        public PlayerBaseEquipState PlayerBaseEquipState { get; protected set; }
        public PlayerBaseReloadState PlayerBaseReloadState { get; protected set; }
        public PlayerBaseFireState PlayerBaseComboState { get; protected set; }

        public PlayerCombatStateMachine(Player player)
        {
            Player = player;
            
            AliveEntityCombatStateReusableData = new AliveEntityCombatStateReusableData();
        }

        public override void Update()
        {
            Debug.Log(CurrentState);
            base.Update();
        }

        public override List<IState> ChangeState(ICombatState newState)
        {
            newState.SetData(this);
            return base.ChangeState(newState);
        }

        public void ExitCurrentState()
        {
            CurrentState.Exit();
        }
    }
}