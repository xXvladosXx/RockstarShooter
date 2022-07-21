using Characters.Player.Data.States.Airborne;
using Characters.Player.Data.States.Grounded;
using GenshinImpactMovementSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Characters.Player.StateMachines.Movement.States
{
    public class PlayerMovementState : IState
    {
        protected readonly PlayerStateMachine PlayerStateMachine;

        protected readonly PlayerGroundedData GroundedData;
        protected readonly AliveEntityAirborneData AirborneData;
        
        private Vector2 _screenCenterPoint;

        public PlayerMovementState(PlayerStateMachine playerPlayerStateMachine)
        {
            PlayerStateMachine = playerPlayerStateMachine;

            GroundedData = PlayerStateMachine.Player.StateData.GroundedData;
            AirborneData = PlayerStateMachine.Player.StateData.AirborneData;
            
            _screenCenterPoint = new Vector2(Screen.width / 2f, Screen.height / 2f);

            InitializeData();
        }

        public virtual void Enter()
        {
            AddInputActionsCallbacks();
        }

        public virtual void Exit()
        {
            RemoveInputActionsCallbacks();
        }

        public virtual void HandleInput()
        {
            ReadMovementInput();
        }

        public virtual void Update()
        {
            PlayerStateMachine.ReusableData.SmoothModifier = Mathf.Clamp(PlayerStateMachine.ReusableData.SmoothModifier, 0, 1);
            
            PlayerStateMachine.ReusableData.MouseWorldPosition = Vector3.zero;
            
            var ray = PlayerStateMachine.Player.MainCamera.ScreenPointToRay(_screenCenterPoint);
            if (Physics.Raycast(ray, out RaycastHit raycastHit, Mathf.Infinity,
                    PlayerStateMachine.Player.LayerData.MouseColliderLayerMask))
            {
                PlayerStateMachine.Player.Aim.transform.position = raycastHit.point;
                PlayerStateMachine.ReusableData.MouseWorldPosition = raycastHit.point;
            }
        }

        public virtual void FixedUpdate()
        {
            Move();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (PlayerStateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);

                return;
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (PlayerStateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGroundExited(collider);

                return;
            }
        }

        public virtual void OnAnimationEnterEvent()
        {
        }

        public virtual void OnAnimationExitEvent()
        {
        }

        public virtual void OnAnimationTransitionEvent()
        {
        }

        private void InitializeData()
        {
            SetBaseRotationData();
        }

        protected void SetBaseRotationData()
        {
            PlayerStateMachine.ReusableData.RotationData = GroundedData.BaseRotationData;

            PlayerStateMachine.ReusableData.TimeToReachTargetRotation = PlayerStateMachine.ReusableData.RotationData.TargetRotationReachTime;
        }

        protected void StartAnimation(int animationHash)
        {
            PlayerStateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash)
        {
            PlayerStateMachine.Player.Animator.SetBool(animationHash, false);
        }

        protected void SetSpeedAnimation(int animationHash, float value, float dampTime)
        {
            PlayerStateMachine.Player.Animator.SetFloat(animationHash, value, dampTime, UnityEngine.Time.deltaTime);
        }
        
        protected virtual void AddInputActionsCallbacks()
        {
            PlayerStateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            PlayerStateMachine.Player.Input.PlayerActions.Look.started += OnMouseMovementStarted;

            PlayerStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
            PlayerStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
            
            PlayerStateMachine.Player.Input.PlayerActions.Aim.performed += OnAimPerformed;
        }

        

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerStateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            PlayerStateMachine.Player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;

            PlayerStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
            PlayerStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            
            PlayerStateMachine.Player.Input.PlayerActions.Aim.performed -= OnAimPerformed;
        }

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            PlayerStateMachine.ReusableData.ShouldWalk = !PlayerStateMachine.ReusableData.ShouldWalk;
        }

        private void OnMouseMovementStarted(InputAction.CallbackContext context)
        {
        }

        protected virtual void OnMovementPerformed(InputAction.CallbackContext context)
        {
        }

        protected virtual void OnMovementCanceled(InputAction.CallbackContext context)
        {
        }
        protected virtual void OnAimPerformed(InputAction.CallbackContext obj)
        {
            PlayerStateMachine.ChangeState(PlayerStateMachine.PlayerAimingState);
        }
        private void ReadMovementInput()
        {
            PlayerStateMachine.ReusableData.MovementInput = PlayerStateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        private void Move()
        {
            if (PlayerStateMachine.ReusableData.MovementInput == Vector2.zero || PlayerStateMachine.ReusableData.MovementSpeedModifier == 0f)
            {
                return;
            }

            Vector3 movementDirection = GetMovementInputDirection();

            float targetRotationYAngle = Rotate(movementDirection);

            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);

            float movementSpeed = GetSmoothMovementSpeed();

            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerStateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        protected float GetSmoothMovementSpeed()
        {
            PlayerStateMachine.ReusableData.SmoothModifier += Time.deltaTime;

            float movementSpeed = GetMovementSpeed();

            movementSpeed = Mathf.Lerp(0, GetMovementSpeed(), PlayerStateMachine.ReusableData.SmoothModifier);

            return movementSpeed;
        }


        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(PlayerStateMachine.ReusableData.MovementInput.x, 0f, PlayerStateMachine.ReusableData.MovementInput.y);
        }

        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle != PlayerStateMachine.ReusableData.CurrentTargetRotation.y)
            {
                UpdateTargetRotationData(directionAngle);
            }

            return directionAngle;
        }

        private float GetDirectionAngle(Vector3 direction)
        {
            float directionAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;

            if (directionAngle < 0f)
            {
                directionAngle += 360f;
            }

            return directionAngle;
        }

        private float AddCameraRotationToAngle(float angle)
        {
            angle += PlayerStateMachine.Player.MainCameraTransform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
            PlayerStateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

            PlayerStateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = PlayerStateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle == PlayerStateMachine.ReusableData.CurrentTargetRotation.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, PlayerStateMachine.ReusableData.CurrentTargetRotation.y, ref PlayerStateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, PlayerStateMachine.ReusableData.TimeToReachTargetRotation.y - PlayerStateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            PlayerStateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            PlayerStateMachine.Player.Rigidbody.MoveRotation(targetRotation);
        }

        protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
        {
            return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            float movementSpeed = GroundedData.BaseSpeed * PlayerStateMachine.ReusableData.MovementSpeedModifier;

            if (shouldConsiderSlopes)
            {
                movementSpeed *= PlayerStateMachine.ReusableData.MovementOnSlopesSpeedModifier;
            }

            return movementSpeed;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = PlayerStateMachine.Player.Rigidbody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, PlayerStateMachine.Player.Rigidbody.velocity.y, 0f);
        }

        protected virtual void OnContactWithGround(Collider collider)
        {
        }

        protected virtual void OnContactWithGroundExited(Collider collider)
        {
        }


        protected void ResetVelocity()
        {
            PlayerStateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerStateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
        }

        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerStateMachine.Player.Rigidbody.AddForce(-playerHorizontalVelocity * PlayerStateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            PlayerStateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * PlayerStateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected bool IsMovingHorizontally(float minimumMagnitude = 0.1f)
        {
            Vector3 playerHorizontaVelocity = GetPlayerHorizontalVelocity();

            Vector2 playerHorizontalMovement = new Vector2(playerHorizontaVelocity.x, playerHorizontaVelocity.z);

            return playerHorizontalMovement.magnitude > minimumMagnitude;
        }

        protected bool IsMovingUp(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y > minimumVelocity;
        }

        protected bool IsMovingDown(float minimumVelocity = 0.1f)
        {
            return GetPlayerVerticalVelocity().y < -minimumVelocity;
        }
    }
}