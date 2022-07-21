using System.Collections.Generic;
using UnityEngine;

namespace AnimatorStateMachine.StateMachine
{
    public interface IState
    {
        public List<IState> Enter();
        public void Exit();
        public void HandleInput();
        public void Update();
        public void FixedUpdate();
        public void OnTriggerEnter(Collider collider);
        public void OnTriggerExit(Collider collider);
        public void OnAnimationEnterEvent();
        public void OnAnimationExitEvent();
        public void OnAnimationTransitionEvent();
    }
}