using StateMachine.Player.StateMachines.Combat.Unarmed;

namespace StateMachine.Player.StateMachines.Combat
{
    public class PlayerMeleeCombatStateMachine : PlayerCombatStateMachine
    {
        public PlayerMeleeCombatStateMachine(Player player) : base(player)
        {
            PlayerBaseIdleState = new MeleeIdleState();
            PlayerBaseFireState = new MeleeFireState();
            PlayerBaseAimState = new MeleeAimState();
            PlayerBaseReloadState = new MeleeReloadState();
            PlayerBaseEquipState = new MeleeEquipState();
            PlayerBaseDashState = new MeleeDashState();
            PlayerBaseComboState = new MeleeComboState();
        }
    }
}