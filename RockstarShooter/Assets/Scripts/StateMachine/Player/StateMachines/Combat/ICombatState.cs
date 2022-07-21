using AnimatorStateMachine.StateMachine;

namespace StateMachine.Player.StateMachines.Combat
{
    public interface ICombatState : IState
    {
        void SetData(PlayerCombatStateMachine playerCombatStateMachine);
    }
}