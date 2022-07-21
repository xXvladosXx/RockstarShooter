using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement;
using Characters.Player.StateMachines.Movement.States;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseRifleState : ICombatState
    {
        protected readonly PlayerCombatStateMachine PlayerCombatStateMachine;


        public PlayerBaseRifleState(PlayerRifleStateMachine playerRifleStateMachine)
        {
            PlayerCombatStateMachine = playerRifleStateMachine;
        }
        
      
        List<IState> IState.Enter()
        {
            AddInputActionsCallbacks();
            return null;
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void HandleInput()
        {
            
        }

        public virtual void Update()
        {
        }

        public virtual void FixedUpdate()
        {
        }

        public void OnTriggerEnter(Collider collider)
        {
        }

        public void OnTriggerExit(Collider collider)
        {
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        public void SetData(PlayerCombatStateMachine playerCombatStateMachine)
        {
            throw new System.NotImplementedException();
        }

        protected virtual void AddInputActionsCallbacks()
        {
            PlayerCombatStateMachine.Player.Input.PlayerActions.Aim.performed += OnAimPerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.performed += OnFirePerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.canceled += OnFireCanceled;
        }

        protected virtual void OnFireCanceled(InputAction.CallbackContext obj)
        {
            Debug.Log("FireCanceled");
        }

        protected virtual void OnFirePerformed(InputAction.CallbackContext obj)
        {
            Debug.Log("FirePerformed");
        }

        protected virtual void OnAimPerformed(InputAction.CallbackContext obj)
        {
            Debug.Log("AimPerformed");
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerCombatStateMachine.Player.Input.PlayerActions.Aim.performed -= OnAimPerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.performed -= OnFirePerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.canceled -= OnFireCanceled;
        }
    }
}