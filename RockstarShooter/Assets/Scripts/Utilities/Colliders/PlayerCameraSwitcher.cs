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

        public CinemachinePOV CinemachinePovAim { get; private set; }
        public CinemachinePOV CinemachinePovDefault { get; private set; }
        public CinemachinePOV CurrentCinemachinePov { get; private set; }
        
        public void InitPovCameras()
        {
            CinemachinePovAim = AimCamera.GetCinemachineComponent<CinemachinePOV>();
            CinemachinePovDefault = DefaultCamera.GetCinemachineComponent<CinemachinePOV>();

            CurrentCinemachinePov = CinemachinePovDefault;
        }
        
        public void SwitchToAimCamera()
        {
            CinemachinePovAim.m_VerticalAxis =
                CinemachinePovDefault.m_VerticalAxis;
            CinemachinePovAim.m_HorizontalAxis =
                CinemachinePovDefault.m_HorizontalAxis;
            AimCamera.gameObject.SetActive(true);
            
            CurrentCinemachinePov = CinemachinePovAim;
        }
        
        public void SwitchToDefaultCamera()
        {
            CinemachinePovDefault.m_VerticalAxis =
                CinemachinePovAim.m_VerticalAxis;
            CinemachinePovDefault.m_HorizontalAxis =
                CinemachinePovAim.m_HorizontalAxis;
            AimCamera.gameObject.SetActive(false);
            
            CurrentCinemachinePov = CinemachinePovDefault;
        }
    }
}