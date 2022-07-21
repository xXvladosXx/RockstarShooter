using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerAimingState : PlayerWalkingFiringState
    {
        public PlayerAimingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {            
        }

        public override void Enter()
        {
            base.Enter();
            
            if (PlayerStateMachine.ReusableData.ShouldFire)
            {
                PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAimingFiringState);
            }
        }


        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
            
            PlayerStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
        }
        
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            base.OnDashStarted(context);
            PlayerStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAimingFiringState);
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ReusableData.ShouldFire = false;
        }

        
    }
}