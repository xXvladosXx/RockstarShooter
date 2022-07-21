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
            AimCamera.gameObject.SetActive(true);
        }
        
        public void SwitchToDefaultCamera()
        {
            AimCamera.gameObject.SetActive(false);
        }
    }
}