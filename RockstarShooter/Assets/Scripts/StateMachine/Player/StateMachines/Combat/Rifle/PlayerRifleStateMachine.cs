using Characters.Player.Data.States;
using StateMachine.Player.StateMachines.Combat;

namespace Characters.Player.StateMachines.Movement
{
    public class PlayerRifleStateMachine : PlayerCombatStateMachine
    {
        public AliveEntityRifleStateReusableData ReusableData { get; }
        public PlayerBaseRifleState PlayerBaseRifleState { get; }

        public PlayerRifleStateMachine(StateMachine.Player.Player player, PlayerMovementStateMachine playerMovementStateMachine) : base(player)
        {
            ReusableData = new AliveEntityRifleStateReusableData();
            
            PlayerBaseRifleState = new PlayerBaseRifleState(this);
        }
    }
}