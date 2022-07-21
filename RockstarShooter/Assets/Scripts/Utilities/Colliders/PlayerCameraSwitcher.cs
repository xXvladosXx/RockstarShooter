using System;
using Cinemachine;
using UnityEngine;

namespace Characters.Player.Utilities.Colliders
{
    [Serializable]
    public class PlayerCameraSwitcher
    {
        [field: SerializeField] public CinemachineVirtualCamera AimCamera { get; private set; }
        [field: SerializeField] public CinemachineVirtualCamera DefaultCamera { get; private set; }

        public void SwitchToAimCamera()
        {
            AimCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis =
                DefaultCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis;
            AimCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis =
                DefaultCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis;
            AimCamera.gameObject.SetActive(true);
        }
        
        public void SwitchToDefaultCamera()
        {
            DefaultCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis =
                AimCamera.GetCinemachineComponent<CinemachinePOV>().m_VerticalAxis;
            DefaultCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis =
                AimCamera.GetCinemachineComponent<CinemachinePOV>().m_HorizontalAxis;
            AimCamera.gameObject.SetActive(false);
        }
    }
}