using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerMovingState : PlayerGroundedState
    {
        public PlayerMovingState(PlayerStateMachine playerPlayerStateMachine) : base(playerPlayerStateMachine)
        {
        }

        public override void Enter()
        {
            base.Enter();

            StartAnimation(PlayerStateMachine.Player.AnimationData.MovingParameterHash);
        }

        public override void Update()
        {
            base.Update();
            SetSpeedAnimation(PlayerStateMachine.Player.AnimationData.SpeedParameterHash, 
                PlayerStateMachine.ReusableData.SmoothModifier, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerStateMachine.Player.AnimationData.MovingParameterHash);
        }
    }
}