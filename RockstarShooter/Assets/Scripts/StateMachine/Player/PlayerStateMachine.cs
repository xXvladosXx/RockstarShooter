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
        private PlayerCombatStateMachine PlayerCombatStateMachine { get; }

        public PlayerStateMachine(Player player)
        {
            Player = player;

            ReusableData = new AliveEntityStateReusableData();

           // PlayerCombatStateMachine = new PlayerCombatStateMachine(Player);
            PlayerMovementStateMachine = new PlayerMovementStateMachine(Player);
        }

        public override List<IState> ChangeState(IState newState)
        {
            var states = new List<IState>();

            for (int i = 0; i < states.Count; i++)
            {
                switch (newState)
                {
                    case IMovementState movementState:
                    {
                        var newStates = PlayerMovementStateMachine.ChangeState(movementState);
                        if (newStates != null)
                            states.AddRange(newStates);
                        break;
                    }
                    case ICombatState combatState:
                    {
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

        }

        public override void OnTriggerExit(Collider collider)
        {
            PlayerMovementStateMachine.OnTriggerExit(collider);

        }

        public void StartState()
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
        }

        public override void Update()
        {
            PlayerMovementStateMachine.Update();
        }

        public override void HandleInput()
        {
            PlayerMovementStateMachine.HandleInput();

        }

        public override void PhysicsUpdate()
        {
            PlayerMovementStateMachine.PhysicsUpdate();

        }

        public override void OnAnimationEnterEvent()
        {
            PlayerMovementStateMachine.OnAnimationEnterEvent();

        }

        public override void OnAnimationExitEvent()
        {
            PlayerMovementStateMachine.OnAnimationExitEvent();

        }

        public override void OnAnimationTransitionEvent()
        {
            PlayerMovementStateMachine.OnAnimationTransitionEvent();

        }
    }
}