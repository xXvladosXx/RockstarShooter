using AnimatorStateMachine.StateMachine;

namespace Characters.Player.StateMachines.Movement
{
    public interface IMovementState : IState
    {
        void SetData(PlayerMovementStateMachine playerMovementStateMachine);
    }
}