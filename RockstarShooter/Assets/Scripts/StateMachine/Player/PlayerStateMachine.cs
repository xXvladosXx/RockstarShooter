using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.Data.States;
using Characters.Player.StateMachines.Movement;
using StateMachine.Player.StateMachines.Combat;
using UnityEngine;

namespace StateMachine.Player
{
    public class PlayerStateMachine : StateMachine<IState>
    {
        public Player Player { get; }
        public AliveEntityStateReusableData ReusableData { get; }

        private PlayerMovementStateMachine PlayerMovementStateMachine { get; }
        private PlayerCombatStateMachine PlayerCombatStateMachine { get; set; }

        public PlayerStateMachine(Player player)
        {
            Player = player;

            ReusableData = new AliveEntityStateReusableData();

            PlayerCombatStateMachine = new PlayerMeleeCombatStateMachine(Player);
            PlayerMovementStateMachine = new PlayerMovementStateMachine(Player);

            foreach (var movementState in PlayerMovementStateMachine.MovementStates)
            {
                movementState.SetPlayerStateMachine(this);
            }
        }

        public void ChangeCombatStateMachine(PlayerCombatStateMachine playerCombatStateMachine)
        {
            PlayerCombatStateMachine.ExitCurrentState();
            PlayerCombatStateMachine = playerCombatStateMachine;
            ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
        }

        public override List<IState> ChangeState(IState newState)
        {
            var states = new List<IState>() { newState };
            
            for (int i = 0; i < states.Count; i++)
            {
                var state = states[i];
                state.SetPlayerStateMachine(this);
                
                switch (state)
                {
                    case IMovementState movementState:
                    {
                        if(movementState == PlayerMovementStateMachine.GetState()) continue;
                        var newStates = PlayerMovementStateMachine.ChangeState(movementState);
                        if (newStates != null)
                            states.AddRange(newStates);
                        break;
                    }
                    case ICombatState combatState:
                    {
                        if(combatState== PlayerCombatStateMachine.GetState()) continue;
                        var newStates = PlayerCombatStateMachine.ChangeState(combatState);
                        if (newStates != null)
                            states.AddRange(newStates);
                        break;
                    }
                }
            }

            return null;
        }

        public override void OnTriggerEnter(Collider collider)
        {
            PlayerMovementStateMachine.OnTriggerEnter(collider);
            PlayerCombatStateMachine.OnTriggerEnter(collider);
        }

        public override void OnTriggerExit(Collider collider)
        {
            PlayerMovementStateMachine.OnTriggerExit(collider);
            PlayerCombatStateMachine.OnTriggerExit(collider);
        }

        public void StartState()
        {
            ChangeState(PlayerMovementStateMachine.IdlingState);
            ChangeState(PlayerCombatStateMachine.PlayerBaseIdleState);
        }

        public override void Update()
        {
            PlayerMovementStateMachine.Update();
            PlayerCombatStateMachine.Update();
        }

        public override void HandleInput()
        {
            PlayerMovementStateMachine.HandleInput();
            PlayerCombatStateMachine.HandleInput();
        }

        public override void PhysicsUpdate()
        {
            PlayerMovementStateMachine.PhysicsUpdate();
            PlayerCombatStateMachine.PhysicsUpdate();
        }

        public override void OnAnimationEnterEvent()
        {
            PlayerMovementStateMachine.OnAnimationEnterEvent();
            PlayerCombatStateMachine.OnAnimationEnterEvent();
        }

        public override void OnAnimationExitEvent()
        {
            PlayerMovementStateMachine.OnAnimationExitEvent();
            PlayerCombatStateMachine.OnAnimationExitEvent();
        }

        public override void OnAnimationTransitionEvent()
        {
            PlayerMovementStateMachine.OnAnimationTransitionEvent();
            PlayerCombatStateMachine.OnAnimationTransitionEvent();
        }
    }
}