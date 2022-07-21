using Characters.Player.Data.Animations;
using Characters.Player.Data.Layers;
using Characters.Player.Data.ScriptableObjects;
using Characters.Player.StateMachines.Movement;
using Characters.Player.Utilities.Colliders;
using Characters.Player.Utilities.Inputs;
using GenshinImpactMovementSystem;
using UnityEngine;

namespace Characters.Player
{
    [RequireComponent(typeof(PlayerInput))]
    [RequireComponent(typeof(PlayerResizableCapsuleCollider))]
    public class Player : MonoBehaviour
    {
        [field: SerializeField] public AliveEntityStateData StateData { get; private set; }
        [field: SerializeField] public PlayerLayerData LayerData { get; private set; }
        [field: SerializeField] public AliveEntityAnimationData AnimationData { get; private set; }
        [field: SerializeField] public PlayerCameraSwitcher PlayerCameraSwitcher { get; private set; }
        [field: SerializeField] public GameObject Aim { get; set; }

        public Rigidbody Rigidbody { get; private set; }
        public Animator Animator { get; private set; }
        
        public PlayerInput Input { get; private set; }
        public PlayerResizableCapsuleCollider ResizableCapsuleCollider { get; private set; }

        public Transform MainCameraTransform { get; private set; }
        public Camera MainCamera { get; private set; }


        private PlayerStateMachine _stateMachine;

        private void Awake()
        {
            AnimationData.Initialize();

            Rigidbody = GetComponent<Rigidbody>();
            Animator = GetComponentInChildren<Animator>();

            Input = GetComponent<PlayerInput>();
            ResizableCapsuleCollider = GetComponent<PlayerResizableCapsuleCollider>();

            MainCamera = Camera.main;
            MainCameraTransform = MainCamera.transform;

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
        }

        private void FixedUpdate()
        {
            _stateMachine.PhysicsUpdate();
        }

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
    }
}