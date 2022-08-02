using System;
using Characters.Player.Utilities.Colliders;
using Cinemachine;
using StateMachine.Player;
using UnityEngine;

namespace Combat
{
    [RequireComponent(typeof(CinemachineImpulseSource))]
    public class WeaponRecoil : MonoBehaviour
    {
        [SerializeField] private Vector2[] _recoilPattern;
        [SerializeField] private float _duration;
        
        private CinemachineImpulseSource _cameraShake;
        private CinemachineVirtualCamera _cinemachineVirtualCamera;

        private CinemachinePOV _cinemachinePov;
        private PlayerCameraSwitcher _playerCameraSwitcher;

        private float _verticalRecoil;
        private float _horizontalRecoil;

        private float _time;
        
        private int _index;
        public void Init(PlayerCameraSwitcher playerCameraSwitcher)
        {
            _playerCameraSwitcher = playerCameraSwitcher;
        }

        private void Awake()
        {
            _cameraShake = GetComponent<CinemachineImpulseSource>();
        }

        private void Update()
        {
            if (_time > 0)
            {
                _playerCameraSwitcher.CurrentCinemachinePov.m_VerticalAxis.Value += (_verticalRecoil*Time.deltaTime)/_duration;
                _playerCameraSwitcher.CurrentCinemachinePov.m_HorizontalAxis.Value += (_horizontalRecoil*Time.deltaTime)/_duration;

                _time -= Time.deltaTime;
            }
        }

        private int FindNextIndex(int index)
        {
            return (index + 1) % _recoilPattern.Length;
        }
        
        public void GenerateRecoil(AttackData attackData)
        {
            if (attackData.DamageApplier is Player player)
            {
                _time = _duration;
                
                _cameraShake.GenerateImpulse();

                _horizontalRecoil = _recoilPattern[_index].x;
                _verticalRecoil = _recoilPattern[_index].y;

                _index = FindNextIndex(_index);
            }
        }
    }
}