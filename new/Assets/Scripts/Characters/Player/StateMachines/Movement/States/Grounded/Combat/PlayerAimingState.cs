using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerAimingState : PlayerGroundedState
    {
        public PlayerAimingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {            
        }

        public override void Enter()
        {
            base.Enter();
            PlayerStateMachine.Player.PlayerCameraSwitcher.SwitchToAimCamera();
        }

        public override void Update()
        {
            base.Update();
            
            Vector3 worldAimTarget = PlayerStateMachine.ReusableData.MouseWorldPosition;
            var transform = PlayerStateMachine.Player.transform;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            PlayerStateMachine.Player.transform.forward = Vector3.Lerp(transform.forward,
                aimDirection, Time.deltaTime * 20f);
        }

        protected override void AddInputActionsCallbacks()
        {
            base.AddInputActionsCallbacks();
        }

        protected override void RemoveInputActionsCallbacks()
        {
            base.RemoveInputActionsCallbacks();
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        public override void Exit()
        {
            base.Exit();
        }
    }
}