using System.Collections;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.Utilities.Inputs
{
    public class PlayerInput : MonoBehaviour
    {
        public PlayerInputActions InputActions { get; private set; }
        public PlayerInputActions.PlayerActions PlayerActions { get; private set; }
        
        public Vector2 Look { get; private set; }
        public bool Fire { get; private set; }

        private void Awake()
        {
            InputActions = new PlayerInputActions();

            PlayerActions = InputActions.Player;
        }

        private void OnEnable()
        {
            InputActions.Enable();
        }

        public void OnLook(InputValue value)
        {
            LookInput(value.Get<Vector2>());
        }
        
        public void OnFire(InputValue value)
        {
            FireInput(value.isPressed);
        }
        
        public void FireInput(bool newFireState)
        {
            Fire = newFireState;
        }


        public void LookInput(Vector2 newLookDirection)
        {
            Look = newLookDirection;
        }
        private void OnDisable()
        {
            InputActions.Disable();
        }

        public void DisableActionFor(InputAction action, float seconds)
        {
            StartCoroutine(DisableAction(action, seconds));
        }

        private IEnumerator DisableAction(InputAction action, float seconds)
        {
            action.Disable();

            yield return new WaitForSeconds(seconds);

            action.Enable();
        }
    }
}