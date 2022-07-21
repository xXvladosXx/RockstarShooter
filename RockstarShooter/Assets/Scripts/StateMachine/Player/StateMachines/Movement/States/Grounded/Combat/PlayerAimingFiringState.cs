using Bonuses;
using Bonuses.CoreBonuses;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Combat
{
    public class PlayerAimingFiringState : PlayerFiringState
    {
        private AccuracyBonus _accuracyBonus = new AccuracyBonus(50);

        public PlayerAimingFiringState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();
            PlayerStateMachine.Player.BonusesController.AddBonus(_accuracyBonus, Stat.Accuracy);

        }

        protected override void OnFireCanceled(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ReusableData.ShouldFire = false;
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAimingState);
        }

        protected override void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
            PlayerStateMachine.ChangeState(PlayerStateMachine.IdlingState);
        }

        protected override void OnDashStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.Player.PlayerCameraSwitcher.SwitchToDefaultCamera();
            base.OnDashStarted(context);
        }
        
        public override void Exit()
        {
            base.Exit();
            PlayerStateMachine.Player.BonusesController.RemoveBonus(_accuracyBonus, Stat.Accuracy);
        }
    }
}