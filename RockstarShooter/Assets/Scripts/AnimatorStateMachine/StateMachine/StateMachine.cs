using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachine<T> where T : IState
    {
        protected T CurrentState;
        public T GetState() => CurrentState;

        public virtual List<IState> ChangeState(T newState)
        {
            CurrentState?.Exit();

            CurrentState = newState;

            return CurrentState.Enter();
        }

        public virtual void HandleInput()
        {
            CurrentState?.HandleInput();
        }

        public virtual void Update()
        {
            CurrentState?.Update();
        }

        public virtual void PhysicsUpdate()
        {
            CurrentState?.FixedUpdate();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            CurrentState?.OnTriggerEnter(collider);
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            CurrentState?.OnTriggerExit(collider);
        }

        public virtual void OnAnimationEnterEvent()
        {
            CurrentState?.OnAnimationEnterEvent();
        }

        public virtual void OnAnimationExitEvent()
        {
            CurrentState?.OnAnimationExitEvent();
        }

        public virtual void OnAnimationTransitionEvent()
        {
            CurrentState?.OnAnimationTransitionEvent();
        }
    }
}