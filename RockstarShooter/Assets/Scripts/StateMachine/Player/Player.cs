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
using StateMachine.Player.StateMachines.Combat;
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

        public PlayerStateMachine PlayerStateMachine { get; private set; }
        public PlayerRifleStateMachine PlayerCombatStateMachine { get; private set; }
        
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
            
            PlayerStateMachine = new PlayerStateMachine(this);
        }

        private void Start()
        {
            PlayerStateMachine.StartState();
        }

        private void Update()
        {
            PlayerStateMachine.HandleInput();
            PlayerStateMachine.Update();

            Rig.weight = Mathf.Lerp(Rig.weight, RigWeight, Time.deltaTime * 20);   

            print(BonusesController.GetStat(Stat.Accuracy));
        }

        private void FixedUpdate()
        {
            PlayerStateMachine.PhysicsUpdate();
        }

        public RaycastHit? GetRaycastHitFromMainCamera() =>
            Raycaster.GetRaycastOfMiddleScreen(MainCamera,
                LayerData.MouseColliderLayerMask);

        private void OnTriggerEnter(Collider collider)
        {
            PlayerStateMachine.OnTriggerEnter(collider);
        }

        private void OnTriggerExit(Collider collider)
        {
            PlayerStateMachine.OnTriggerExit(collider);
        }

        public void OnAnimationEnterEvent()
        {
            PlayerStateMachine.OnAnimationEnterEvent();
        }

        public void OnAnimationExitEvent()
        {
            PlayerStateMachine.OnAnimationExitEvent();
        }

        public void OnAnimationTransitionEvent()
        {
            PlayerStateMachine.OnAnimationTransitionEvent();
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