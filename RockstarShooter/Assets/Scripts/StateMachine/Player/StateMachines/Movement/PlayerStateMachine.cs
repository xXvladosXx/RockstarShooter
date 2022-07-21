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
    public class PlayerStateMachine : StateMachine.StateMachine
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
        
        public PlayerFiringState PlayerFiringState { get; }
        public PlayerAimingFiringState PlayerAimingFiringState { get; }
        public PlayerAimingState PlayerAimingState { get;  }

        public PlayerStateMachine(StateMachine.Player.Player player)
        {
            Player = player;
            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState(this);
            DashingState = new PlayerDashingState(this);

            WalkingFiringState = new PlayerWalkingFiringState(this);
            RunningState = new PlayerRunningState(this);
            SprintingState = new PlayerSprintingState(this);

            LightStoppingState = new PlayerLightStoppingState(this);
            MediumStoppingState = new PlayerMediumStoppingState(this);
            HardStoppingState = new PlayerHardStoppingState(this);

            LightLandingState = new PlayerLightLandingState(this);
            RollingState = new PlayerRollingState(this);
            HardLandingState = new PlayerHardLandingState(this);

            JumpingState = new PlayerJumpingState(this);
            FallingState = new PlayerFallingState(this);

            PlayerFiringState = new PlayerFiringState(this);
            PlayerAimingState = new PlayerAimingState(this);
            PlayerAimingFiringState = new PlayerAimingFiringState(this);
        }
        
    }
}