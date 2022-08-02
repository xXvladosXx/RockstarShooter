using System;
using UnityEngine;

namespace Characters.Player.Data.Animations
{
    [CreateAssetMenu (menuName = "StateMachine/AnimationData")]
    public class AliveEntityAnimationData : ScriptableObject
    {
        [field: Header(" Movement")] 
        [SerializeField] private string HorizontalParameterName = "Horizontal";
        [SerializeField] private string VerticalParameterName = "Vertical"; 

        [field: Header("Animator Layers")]
        [field: SerializeField]
        public int AimingLayer { get; private set; } = 1;
        public int FiringLayer { get; private set; } = 2;
        public int ReloadLayer { get; private set; } = 3;
        public int EquippingLayer { get; private set; } = 4;

        
        [Header("State Group Parameter Names")]
        [SerializeField] private string _groundedParameterName = "Grounded";
        [SerializeField] private string _movingParameterName = "Moving";
        [SerializeField] private string _stoppingParameterName = "Stopping";
        [SerializeField] private string _landingParameterName = "Landing";
        [SerializeField] private string _airborneParameterName = "Airborne";

        [Header("Grounded Parameter Names")]
        [SerializeField] private string _idleParameterName = "isIdling";
        [SerializeField] private string _dashParameterName = "isDashing";
        [SerializeField] private string _walkParameterName = "isWalking";
        [SerializeField] private string _runParameterName = "isRunning";
        [SerializeField] private string _sprintParameterName = "isSprinting";
        [SerializeField] private string _mediumStopParameterName = "isMediumStopping";
        [SerializeField] private string _hardStopParameterName = "isHardStopping";
        [SerializeField] private string _rollParameterName = "isRolling";
        [SerializeField] private string _hardLandParameterName = "isHardLanding";
        
        [Header("Combat Parameter Names")]
        [SerializeField] private string _aimingParameterName = "isAiming";
        [SerializeField] private string _firingParameterName = "isFiring";
        [SerializeField] private string _equippingParameterName = "isEquipping";
        [SerializeField] private string _reloadingParameterName = "isReloading";
        [SerializeField] private string _comboParameterName = "Combo";

        [SerializeField] private string _speedParameterName = "Speed";

        [Header("Airborne Parameter Names")]
        [SerializeField] private string _fallParameterName = "isFalling";

        public int GroundedParameterHash { get; private set; }
        public int MovingParameterHash { get; private set; }
        public int StoppingParameterHash { get; private set; }
        public int LandingParameterHash { get; private set; }
        public int AirborneParameterHash { get; private set; }

        public int IdleParameterHash { get; private set; }
        public int DashParameterHash { get; private set; }
        public int WalkParameterHash { get; private set; }
        public int RunParameterHash { get; private set; }
        public int SprintParameterHash { get; private set; }
        public int MediumStopParameterHash { get; private set; }
        public int HardStopParameterHash { get; private set; }
        public int RollParameterHash { get; private set; }
        public int HardLandParameterHash { get; private set; }

        public int FallParameterHash { get; private set; }
        public int HorizontalParameterHash { get; private set; }
        public int VerticalParameterHash { get; private set; }

        public int SpeedParameterHash { get; private set; }
        public int AimingParameterHash { get; private set; }
        public int FiringParameterHash { get; private set; }
        public int ComboParameterHash { get; private set; }
        public int ReloadParameterHash { get; private set; }
        public int EquippingParameterHash { get; private set; }

        public void Init()
        {
            GroundedParameterHash = Animator.StringToHash(_groundedParameterName);
            MovingParameterHash = Animator.StringToHash(_movingParameterName);
            StoppingParameterHash = Animator.StringToHash(_stoppingParameterName);
            LandingParameterHash = Animator.StringToHash(_landingParameterName);
            AirborneParameterHash = Animator.StringToHash(_airborneParameterName);

            IdleParameterHash = Animator.StringToHash(_idleParameterName);
            DashParameterHash = Animator.StringToHash(_dashParameterName);
            WalkParameterHash = Animator.StringToHash(_walkParameterName);
            RunParameterHash = Animator.StringToHash(_runParameterName);
            SprintParameterHash = Animator.StringToHash(_sprintParameterName);
            MediumStopParameterHash = Animator.StringToHash(_mediumStopParameterName);
            HardStopParameterHash = Animator.StringToHash(_hardStopParameterName);
            RollParameterHash = Animator.StringToHash(_rollParameterName);
            HardLandParameterHash = Animator.StringToHash(_hardLandParameterName);

            FallParameterHash = Animator.StringToHash(_fallParameterName);

            SpeedParameterHash = Animator.StringToHash(_speedParameterName);

            AimingParameterHash = Animator.StringToHash(_aimingParameterName);
            FiringParameterHash = Animator.StringToHash(_firingParameterName);
            ReloadParameterHash = Animator.StringToHash(_reloadingParameterName);
            EquippingParameterHash = Animator.StringToHash(_equippingParameterName);
            ComboParameterHash = Animator.StringToHash(_comboParameterName);

            HorizontalParameterHash = Animator.StringToHash(HorizontalParameterName);
            VerticalParameterHash = Animator.StringToHash(VerticalParameterName);
        }

    }
}