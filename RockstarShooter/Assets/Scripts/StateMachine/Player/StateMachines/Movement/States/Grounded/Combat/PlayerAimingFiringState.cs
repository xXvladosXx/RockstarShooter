/*using Bonuses;
using Bonuses.CoreBonuses;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerAimingFiringState : PlayerFiringState
    {
        private AccuracyBonus _accuracyBonus = new AccuracyBonus(50);

        public PlayerAimingFiringState(PlayerMovementStateMachine playerMovementPlayerMovementStateMachine) : base(playerMovementPlayerMovementStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerMovementStateMachine.Player.BonusesController.AddBonus(_accuracyBonus, Stat.Accuracy);

        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerMovementStateMachine.ReusableData.ShouldFire = false;
            //PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.PlayerAimingState);
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerMovementStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
            PlayerMovementStateMachine.ChangeState(PlayerMovementStateMachine.IdlingState);
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerMovementStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
            base.OnDashStarted(context);
        }
        
        public override void Exit()
        {
            base.Exit();
            PlayerMovementStateMachine.Player.BonusesController.RemoveBonus(_accuracyBonus, Stat.Accuracy);
        }
    }
}*/