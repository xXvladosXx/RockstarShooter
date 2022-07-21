using System;
using System.Collections.Generic;
using System.Linq;
using Bonuses;
using Bonuses.CoreBonuses;
using Characters.Player.Data.Animations;
using Characters.Player.Data.Layers;
using Characters.Player.Data.ScriptableObjects;
using Characters.Player.StateMachines.Movement;
using Characters.Player.Utilities.Colliders;
using Characters.Player.Utilities.Inputs;
using Combat;
using Data.Stats;
using Data.Stats.Core;
using Inventory;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using Utilities;

namespace StateMachine.Player
{
    [RequireComponent(
        typeof(PlayerInput),
        typeof(PlayerResizableCapsuleCollider),
        typeof(ItemEquipper))]
    public class Player : MonoBehaviour, IModifier
    {
        [field: Header("Characteristics")]
        [field: SerializeField]
        public AliveEntityStatsModifierData AliveEntityStatsModifierData { get; private set; }
        [field: SerializeField] public AliveEntityStatsData AliveEntityStatsData { get; private set; }

        
        [field: SerializeField] public AliveEntityStateData StateData { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AnimationData { get; private set; }

        [field: Header("Cinemachine")]
        [field: SerializeField]
        public PlayerCameraSwitcher PlayerCameraSwitcher { get; private set; }

        [field: SerializeField] public GameObject Aim { get; set; }
        [field: SerializeField] public Rig Rig { get; set; }

        public float RigWeight { get; set; }
        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        public ItemEquipper ItemEquipper { get; private set; }

        public BonusesController BonusesController { get; private set; }
        public PlayerInput Input { get; private set; }
        public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }
        public StatsValueStorage StatsValueStorage { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        public Camera MainCamera { get; private set; }


        private PlayerStateMachine _stateMachine;
        private List<IModifier> _modifiers = new();
        private void Awake()
        {
            AnimationData.Initialize();

            ItemEquipper = GetComponent<ItemEquipper>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            Input = GetComponent<PlayerInput>();

            StatsValueStorage = new StatsValueStorage(AliveEntityStatsModifierData, AliveEntityStatsData);
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            MainCamera = Camera.main;
            MainCameraTransform = MainCamera.transform;

            _modifiers.Add(this);
            
            BonusesController = new BonusesController(_modifiers);
            _stateMachine = new PlayerStateMachine(this);
        }

        private void Start()
        {
            _stateMachine.ChangeState(_stateMachine.IdlingState);
        }

        private void Update()
        {
            _stateMachine.HandleInput();
            _stateMachine.Update();

            Rig.weight = Mathf.Lerp(Rig.weight, RigWeight, Time.deltaTime * 20);   

            print(BonusesController.GetStat(Stat.Accuracy));
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsUpdate();
        }

        public RaycastHit? GetRaycastHitFromMainCamera() =>
            Raycaster.GetRaycastOfMiddleScreen(MainCamera,
                LayerData.MouseColliderLayerMask);

        private void OnTriggerEnter(Collider collider)
        {
            _stateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            _stateMachine.OnTriggerExit(collider);
        }

        public void OnAnimationEnterEvent()
        {
            _stateMachine.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            _stateMachine.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            _stateMachine.OnAnimationTransitionEvent();
        }

        public void FireFromCurrentWeapon()
        {
            AttackData attackData = new AttackData
            {
                RaycastHit = GetRaycastHitFromMainCamera(),
                AccuracyModifier = BonusesController.GetStat(Stat.Accuracy)
            };
            
            ItemEquipper.FireFromCurrentWeapon(attackData);
        }

        public event Action OnStatModified;

        public IEnumerable<IBonus> AddBonus(Stat[] stats)
        {
            IBonus BonusTo(Stat stats)
            {
                return stats switch
                {
                    Stat.Damage => new DamageBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.Damage)),
                    Stat.Health => new HealthBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.Health)),
                    Stat.CriticalChance => new CriticalChanceBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.CriticalChance)),
                    Stat.CriticalDamage => new CriticalDamageBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.CriticalDamage)),
                    Stat.ManaRegeneration => new ManaRegenerationBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.ManaRegeneration)),
                    Stat.HealthRegeneration => new HealthRegenerationBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.HealthRegeneration)),
                    Stat.AttackSpeed => new AttackSpeedBonus(1),
                    Stat.Mana => new ManaBonus(
                        StatsValueStorage.GetCalculatedStat(Stat.Mana)),
                    Stat.Accuracy => new AccuracyBonus(100),
                    _ => throw new ArgumentOutOfRangeException(nameof(stats), stats, null)
                };
            }

            return stats.Select(BonusTo);
        }
    }
}