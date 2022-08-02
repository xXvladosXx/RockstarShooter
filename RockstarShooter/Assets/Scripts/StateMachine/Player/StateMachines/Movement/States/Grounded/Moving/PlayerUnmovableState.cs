using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.StateMachines.Movement.States.Grounded;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;

namespace StateMachine.Player.StateMachines.Movement.States.Grounded.Moving
{
    public class PlayerUnmovableState : PlayerWalkingState
    {
        public override void Update()
        {
            base.Update();
            
            LockToAim();
        }

        public override void FixedUpdate()
        {
            Float();
        }

        public override void Exit()
        {
            base.Exit();
            
            StopAnimation(PlayerStateMachine.Player.AnimationData.SprintParameterHash);
        }
    }
}