using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace GenshinImpactMovementSystem
{
    public class CameraCursor : MonoBehaviour
    {
        [SerializeField] private InputActionReference cameraToggleInputAction;
        [SerializeField] private bool startHidden;

        [SerializeField] private CinemachineInputProvider inputProvider;
        [SerializeField] private bool disableCameraLookOnCursorVisible;
        [SerializeField] private bool disableCameraZoomOnCursorVisible;

        [SerializeField] private bool fixedCinemachineVersion;

        private void Awake()
        {
            cameraToggleInputAction.action.started += OnCameraCursorToggled;

            if (startHidden)
            {
                ToggleCursor();
            }
        }

        private void OnEnable()
        {
            cameraToggleInputAction.asset.Enable();
        }

        private void OnDisable()
        {
            cameraToggleInputAction.asset.Disable();
        }

        private void OnCameraCursorToggled(InputAction.CallbackContext context)
        {
            ToggleCursor();
        }

        private void ToggleCursor()
        {
            Cursor.visible = !Cursor.visible;

            if (!Cursor.visible)
            {
                Cursor.lockState = CursorLockMode.Locked;

                if (!fixedCinemachineVersion)
                {
                    inputProvider.enabled = true;

                    return;
                }

                inputProvider.XYAxis.action.Enable();
                inputProvider.ZAxis.action.Enable();

                return;
            }

            Cursor.lockState = CursorLockMode.None;

            if (!fixedCinemachineVersion)
            {
                inputProvider.enabled = false;

                return;
            }

            if (disableCameraLookOnCursorVisible)
            {
                inputProvider.XYAxis.action.Disable();
            }

            if (disableCameraZoomOnCursorVisible)
            {
                inputProvider.ZAxis.action.Disable();
            }
        }
    }
}