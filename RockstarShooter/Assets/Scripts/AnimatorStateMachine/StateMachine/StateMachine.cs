using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace StateMachine
{
    public abstract class StateMachine<T> where T : IState
    {
        private T _currentState;

        public virtual List<IState> ChangeState(T newState)
        {
            _currentState?.Exit();

            _currentState = newState;

            return _currentState.Enter();
        }

        public virtual void HandleInput()
        {
            _currentState?.HandleInput();
        }

        public virtual void Update()
        {
            _currentState?.Update();
        }

        public virtual void PhysicsUpdate()
        {
            _currentState?.FixedUpdate();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            _currentState?.OnTriggerEnter(collider);
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            _currentState?.OnTriggerExit(collider);
        }

        public virtual void OnAnimationEnterEvent()
        {
            _currentState?.OnAnimationEnterEvent();
        }

        public virtual void OnAnimationExitEvent()
        {
            _currentState?.OnAnimationExitEvent();
        }

        public virtual void OnAnimationTransitionEvent()
        {
            _currentState?.OnAnimationTransitionEvent();
        }
    }
}