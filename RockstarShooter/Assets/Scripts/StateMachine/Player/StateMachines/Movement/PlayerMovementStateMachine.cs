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
using StateMachine.Player.StateMachines.Movement.States.Grounded.Moving;
using UnityEngine;

namespace Characters.Player.StateMachines.Movement
{
    public class PlayerMovementStateMachine : StateMachine.StateMachine<IMovementState>
    {
        public StateMachine.Player.Player Player { get; }
        public AliveEntityStateReusableData ReusableData { get; }

        public PlayerIdlingState IdlingState { get; }
        public PlayerDashingState DashingState { get; }

        public PlayerWalkingState WalkingState { get; }
        public PlayerRunningState RunningState { get; }
        public PlayerSprintingState SprintingState { get; }
        public PlayerUnmovableState PlayerUnmovableState { get; }

        public PlayerLightStoppingState LightStoppingState { get; }
        public PlayerMediumStoppingState MediumStoppingState { get; }
        public PlayerHardStoppingState HardStoppingState { get; }

        public PlayerLightLandingState LightLandingState { get; }
        public PlayerRollingState RollingState { get; }
        public PlayerHardLandingState HardLandingState { get; }

        public PlayerJumpingState JumpingState { get; }
        public PlayerFallingState FallingState { get; }

        public List<IMovementState> MovementStates { get; } = new();


        public PlayerMovementStateMachine(StateMachine.Player.Player player)
        {
            Player = player;
            ReusableData = new AliveEntityStateReusableData();

            IdlingState = new PlayerIdlingState();
            DashingState = new PlayerDashingState();

            WalkingState = new PlayerWalkingState();
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

            PlayerUnmovableState = new PlayerUnmovableState();
            
            MovementStates.Add(IdlingState);
            MovementStates.Add(DashingState);
            MovementStates.Add(WalkingState);
            MovementStates.Add(RunningState);
            MovementStates.Add(SprintingState);
            MovementStates.Add(LightStoppingState);
            MovementStates.Add(MediumStoppingState);
            MovementStates.Add(HardStoppingState);
            MovementStates.Add(LightLandingState);
            MovementStates.Add(HardLandingState);
            MovementStates.Add(RollingState);
            MovementStates.Add(JumpingState);
            MovementStates.Add(FallingState);
        }

        public override void Update()
        {
            Debug.Log(CurrentState);
            base.Update();
        }

        public override List<IState> ChangeState(IMovementState newState)
        {
            newState.SetData(this);
            return base.ChangeState(newState);
        }
    }
}