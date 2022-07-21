using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerMovingState : PlayerGroundedState
    {

        public override List<IState> Enter()
        {
            base.Enter();

            StartAnimation(PlayerMovementStateMachine.Player.AnimationData.MovingParameterHash);
            
            return null;

        }

        public override void Update()
        {
            base.Update();
            SetSpeedAnimation(PlayerMovementStateMachine.Player.AnimationData.SpeedParameterHash, 
                PlayerMovementStateMachine.ReusableData.SmoothModifier, 0.1f);
        }

        public override void Exit()
        {
            base.Exit();

            StopAnimation(PlayerMovementStateMachine.Player.AnimationData.MovingParameterHash);
        }
    }
}