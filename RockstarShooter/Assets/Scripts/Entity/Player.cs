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
using Combat;
using Data.Stats;
using Data.Stats.Core;
using Entity;
using Inventory;
using Inventory.Core;
using StateMachine.Player.StateMachines.Combat;
using UnityEngine;
using UnityEngine.Animations.Rigging;
using UnityEngine.InputSystem;
using Utilities;
using PlayerInput = Characters.Player.Utilities.Inputs.PlayerInput;

namespace StateMachine.Player
{
    [RequireComponent(
        typeof(PlayerInput),
        typeof(PlayerResizableCapsuleCollider),
        typeof(ItemEquipper))]
    public class Player : MonoBehaviour, IModifier, IDamageApplier
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
        [field: SerializeField] public Rig LeftHandRig { get; set; }

        public float RightHandBodyRigWeight { get; set; }
        public float LeftRigWeight { get; set; } = 1;
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
        public AttackMaker AttackMaker { get; private set; }
        
        private List<IModifier> _modifiers = new();
        
        public event Action OnStatModified;

        private void Awake()
        {
            Input = GetComponent<PlayerInput>();
            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();
            ItemEquipper = GetComponent<ItemEquipper>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            PlayerCameraSwitcher.InitPovCameras();
            AnimationData.Init();
            Input.Init();
            ItemEquipper.InitData(PlayerCameraSwitcher);

            AttackMaker = new AttackMaker(Input.InputActions, ItemEquipper, this);

            StatsValueStorage = new StatsValueStorage(AliveEntityStatsModifierData, AliveEntityStatsData);

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
            
            Rig.weight = Mathf.Lerp(Rig.weight, RightHandBodyRigWeight, Time.deltaTime * 20);
            LeftHandRig.weight = Mathf.Lerp(LeftHandRig.weight, LeftRigWeight, Time.deltaTime *5);
            print(BonusesController.GetStat(Stat.Accuracy));

            if (Input.PlayerActions.ChangeWeaponRight.WasPressedThisFrame())
            {
                ItemEquipper.ChangeWeaponIndex(1);
            }

            if (Input.PlayerActions.ChangeWeaponLeft.WasPressedThisFrame())
            {
                ItemEquipper.ChangeWeaponIndex(-1);
            }
        }

        private void FixedUpdate()
        {
            PlayerStateMachine.PhysicsUpdate();
        }

        public RaycastHit? GetRaycastHit(Vector3 offset = new ()) =>
            Raycaster.GetRaycastOfMiddleScreen(MainCamera,
                LayerData.MouseColliderLayerMask, offset);

        public BonusesController DamagerBonusesController() => BonusesController;

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

        public void OnMagazineDrop()
        {
            ItemEquipper.DropMagazine();
        }

        public void OnMagazineTake()
        {
            ItemEquipper.TakeMagazine();
        }
        
        public void MagazineLoaded()
        {
            ItemEquipper.MagazineLoaded();
        }

        public void MagazineReloaded()
        {
            ItemEquipper.WeaponReloaded();
        }

        public void ChangeWeapon()
        {
            ItemEquipper.ChangeWeapon(Animator);
            PlayerCombatStateMachine playerRifleCombatStateMachine = null;

            switch (ItemEquipper.CurrentWeapon.WeaponItem.ItemType)
            {
                case ItemType.Rifle:
                    playerRifleCombatStateMachine = new PlayerRifleCombatStateMachine(this);
                    break;
                case ItemType.SteelArms:
                    break;
                case ItemType.Sword:
                    playerRifleCombatStateMachine = new PlayerMeleeCombatStateMachine(this);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            PlayerStateMachine.ChangeCombatStateMachine(playerRifleCombatStateMachine);
        }
    }
}