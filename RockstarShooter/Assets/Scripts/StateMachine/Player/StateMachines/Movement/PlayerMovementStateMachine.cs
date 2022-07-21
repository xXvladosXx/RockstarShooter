using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Characters.Player.Data.States;
using Characters.Player.StateMachines.Movement.States.Airborne;
using Characters.Player.StateMachines.Movement.States.Grounded;
using Characters.Player.StateMachines.Movement.States.Grounded.Combat;
using Characters.Player.StateMachines.Movement.States.Grounded.Landing;
using Characters.Player.StateMachines.Movement.States.Grounded.Moving;
using Characters.Player.StateMachines.Movement.States.Grounded.Stopping;
using GenshinImpactMovementSystem;

namespace Characters.Player.StateMachines.Movement
{
    public class PlayerMovementStateMachine : StateMachine.StateMachine<IMovementState>
    {
        public StateMachine.Player.Player Player { get; }
        public AliveEntityStateReusableData ReusableData { get; }

        public PlayerIdlingState IdlingState { get; }
        public PlayerDashingState DashingState { get; }

        public PlayerWalkingFiringState WalkingFiringState { get; }
        public PlayerRunningState RunningState { get; }
        public PlayerSprintingState SprintingState { get; }

        public PlayerLightStoppingState LightStoppingState { get; }
        public PlayerMediumStoppingState MediumStoppingState { get; }
        public PlayerHardStoppingState HardStoppingState { get; }

        public PlayerLightLandingState LightLandingState { get; }
        public PlayerRollingState RollingState { get; }
        public PlayerHardLandingState HardLandingState { get; }

        public PlayerJumpingState JumpingState { get; }
        public PlayerFallingState FallingState { get; }
        
        //public PlayerFiringState PlayerFiringState { get; }
        //public PlayerAimingFiringState PlayerAimingFiringState { get; }
        //public PlayerAimingState PlayerAimingState { get;  }

        public PlayerMovementStateMachine(StateMachine.Player.Player player)
        {
            Player = player;
            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState();
            DashingState = new PlayerDashingState();

            WalkingFiringState = new PlayerWalkingFiringState();
            RunningState = new PlayerRunningState();
            SprintingState = new PlayerSprintingState();

            LightStoppingState = new PlayerLightStoppingState();
            MediumStoppingState = new PlayerMediumStoppingState();
            HardStoppingState = new PlayerHardStoppingState();

            LightLandingState = new PlayerLightLandingState();
            RollingState = new PlayerRollingState();
            HardLandingState = new PlayerHardLandingState();

            JumpingState = new PlayerJumpingState();
            FallingState = new PlayerFallingState();

            //PlayerFiringState = new PlayerFiringState(this);
            //PlayerAimingState = new PlayerAimingState(this);
            //PlayerAimingFiringState = new PlayerAimingFiringState(this);
        }

        public override List<IState> ChangeState(IMovementState newState)
        {
            newState.SetData(this);
            return base.ChangeState(newState);
        }
    }
}