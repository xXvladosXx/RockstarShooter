/*using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerAimingState : PlayerWalkingFiringState
    {
        public PlayerAimingState(PlayerMovementStateMachine playerMovementPlayerMovementStateMachine) : base(playerMovementPlayerMovementStateMachine)
        {            
        }

        public override void Enter()
        {
            base.Enter();
            
            if (PlayerMovementStateMachine.ReusableData.ShouldFire)
            {
                //PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.PlayerAimingFiringState);
            }
        }


        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
            
            PlayerMovementStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
        }
        
        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            base.OnDashStarted(context);
            PlayerMovementStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
        }

        protected override void OnFirePerformed(InputAction.CallbackContext obj)
        {
            //PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.PlayerAimingFiringState);
        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerMovementStateMachine.ReusableData.ShouldFire = false;
        }

        
    }
}*/