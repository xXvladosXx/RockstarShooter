using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerBaseCombatState : ICombatState
    {
        protected PlayerCombatStateMachine PlayerCombatStateMachine { get; private set; }
        protected PlayerStateMachine PlayerStateMachine { get; private set; }

        public void SetPlayerStateMachine(PlayerStateMachine playerStateMachine)
        {
            PlayerStateMachine = playerStateMachine;
        }
        
        public void SetData(PlayerCombatStateMachine playerCombatStateMachine)
        {
            PlayerCombatStateMachine = playerCombatStateMachine;
        }
       
        public virtual List<IState> Enter()
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
            ReadMovementInput();
        }

        private void ReadMovementInput()
        {
            PlayerCombatStateMachine.AliveEntityCombatStateReusableData.MovementInput =
                PlayerCombatStateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        public virtual void Update()
        {
            if (PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseAimLayer)
            {
                PlayerCombatStateMachine.AliveEntityCombatStateReusableData.AimLayerWeight -= Time.deltaTime*10;
                PlayerCombatStateMachine.AliveEntityCombatStateReusableData.AimLayerWeight = Mathf.Clamp(
                    PlayerCombatStateMachine.AliveEntityCombatStateReusableData.AimLayerWeight, 0f, 1f);
                PlayerCombatStateMachine.Player.Animator.SetLayerWeight(
                    PlayerCombatStateMachine.Player.AnimationData.AimingLayer,
                    PlayerCombatStateMachine.AliveEntityCombatStateReusableData.AimLayerWeight);
            }
            
            if (PlayerCombatStateMachine.AliveEntityCombatStateReusableData.ShouldDecreaseEquipLayer)
            {
                PlayerCombatStateMachine.AliveEntityCombatStateReusableData.EquipLayerWeight -= Time.deltaTime;
                PlayerCombatStateMachine.AliveEntityCombatStateReusableData.EquipLayerWeight = Mathf.Clamp(
                    PlayerCombatStateMachine.AliveEntityCombatStateReusableData.EquipLayerWeight, 0f, 1f);
                PlayerCombatStateMachine.Player.Animator.SetLayerWeight(
                    PlayerCombatStateMachine.Player.AnimationData.EquippingLayer,
                    PlayerCombatStateMachine.AliveEntityCombatStateReusableData.EquipLayerWeight);
            }
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
        

        protected void StartAnimation(int animationHash)
        {
            PlayerCombatStateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash)
        {
            PlayerCombatStateMachine.Player.Animator.SetBool(animationHash, false);
        }
        
        protected void SetSpeedAnimation(int animationHash, float value, float dampTime)
        {
            PlayerCombatStateMachine.Player.Animator.SetFloat(animationHash, value, dampTime, UnityEngine.Time.deltaTime);
        }
        
        protected void SetSpeedAnimation(int animationHash, float value)
        {
            PlayerCombatStateMachine.Player.Animator.SetFloat(animationHash, value);
        }

        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(PlayerCombatStateMachine.AliveEntityCombatStateReusableData.MovementInput.x, 0f, 
                PlayerCombatStateMachine.AliveEntityCombatStateReusableData.MovementInput.y);
        }
        
        protected virtual void AddInputActionsCallbacks()
        {
            PlayerCombatStateMachine.Player.Input.PlayerActions.Aim.performed += OnAimPerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.EquipWeapon.performed += OnEquipPerformed;
            
            PlayerCombatStateMachine.Player.Input.PlayerActions.Reload.performed += OnReloadPerformed;
            PlayerCombatStateMachine.Player.ItemEquipper.CurrentWeapon.OnReload += OnReloadPerformed;
            
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.performed += OnFirePerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.canceled += OnFireCanceled;
        }

        protected virtual void OnReloadPerformed()
        {
        }

        protected virtual void OnReloadPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerCombatStateMachine.PlayerBaseReloadState);
        }

        protected virtual void OnFireCanceled(InputAction.CallbackContext obj)
        {
        }
        protected virtual void OnEquipPerformed(InputAction.CallbackContext obj)
        {
            
        }
        protected virtual void OnFirePerformed(InputAction.CallbackContext obj)
        {
        }

        protected virtual void OnAimPerformed(InputAction.CallbackContext obj)
        {
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerCombatStateMachine.Player.Input.PlayerActions.Aim.performed -= OnAimPerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.EquipWeapon.performed -= OnEquipPerformed;

            PlayerCombatStateMachine.Player.Input.PlayerActions.Reload.performed -= OnReloadPerformed;
            PlayerCombatStateMachine.Player.ItemEquipper.CurrentWeapon.OnReload -= OnReloadPerformed;
            
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.performed -= OnFirePerformed;
            PlayerCombatStateMachine.Player.Input.PlayerActions.Fire.canceled -= OnFireCanceled;
        }
    }
}