using UnityEngine;

namespace Characters.Player.Utilities.Animations
{
    public class PlayerAnimationEventTrigger : MonoBehaviour
    {
        private StateMachine.Player.Player _player;

        private void Awake()
        {
            _player = GetComponent<StateMachine.Player.Player>();
        }

        public void OnAnimationEnterEvent()
        {
            _player.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {

            _player.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            _player.OnAnimationTransitionEvent();
        }

        public void Fire()
        {
            _player.FireFromCurrentWeapon();
        }
        
    }
}