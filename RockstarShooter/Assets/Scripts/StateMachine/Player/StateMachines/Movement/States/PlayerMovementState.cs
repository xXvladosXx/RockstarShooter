using System.Collections.Generic;
using AnimatorStateMachine.StateMachine;
using Bonuses;
using Characters.Player.Data.States.Airborne;
using Characters.Player.Data.States.Grounded;
using GenshinImpactMovementSystem;
using StateMachine.Player;
using UnityEngine;
using UnityEngine.InputSystem;
using Utilities;

namespace Characters.Player.StateMachines.Movement.States
{
    public class PlayerMovementState : IMovementState
    {
        protected PlayerMovementStateMachine PlayerMovementStateMachine { get; private set; }
        protected PlayerStateMachine PlayerStateMachine { get; private set; }
        
        protected PlayerGroundedData GroundedData { get; private set; }
        protected AliveEntityAirborneData AirborneData { get; private set; }
        
        public void SetData(PlayerMovementStateMachine playerMovementStateMachine)
        {
            PlayerMovementStateMachine = playerMovementStateMachine;

            GroundedData = PlayerMovementStateMachine.Player.StateData.GroundedData;
            AirborneData = PlayerMovementStateMachine.Player.StateData.AirborneData;
            
            InitializeData();
        }

        public void SetPlayerStateMachine(PlayerStateMachine playerStateMachine)
        {
            PlayerStateMachine = playerStateMachine;
        }

        public virtual List<IState> Enter()
        {
            AddInputActionsCallbacks();

            return null;
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
            PlayerMovementStateMachine.ReusableData.SmoothModifier = Mathf.Clamp(PlayerMovementStateMachine.ReusableData.SmoothModifier,
                0,PlayerMovementStateMachine.ReusableData.MaxSmoothModifier);
            
            PlayerMovementStateMachine.ReusableData.MouseWorldPosition = Vector3.zero;

            RaycastHit? raycastHit = PlayerMovementStateMachine.Player.GetRaycastHit();

            PlayerMovementStateMachine.Player.Aim.transform.position = (Vector3) raycastHit?.point;
                PlayerMovementStateMachine.ReusableData.MouseWorldPosition = (Vector3) raycastHit?.point;
        }

        public virtual void FixedUpdate()
        {
            MoveAccordingCameraPosition();
        }

        public virtual void OnTriggerEnter(Collider collider)
        {
            if (PlayerMovementStateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
            {
                OnContactWithGround(collider);

                return;
            }
        }

        public virtual void OnTriggerExit(Collider collider)
        {
            if (PlayerMovementStateMachine.Player.LayerData.IsGroundLayer(collider.gameObject.layer))
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
            PlayerMovementStateMachine.ReusableData.RotationData = GroundedData.BaseRotationData;

            PlayerMovementStateMachine.ReusableData.TimeToReachTargetRotation = PlayerMovementStateMachine.ReusableData.RotationData.TargetRotationReachTime;
        }

        protected void StartAnimation(int animationHash)
        {
            PlayerMovementStateMachine.Player.Animator.SetBool(animationHash, true);
        }

        protected void StopAnimation(int animationHash)
        {
            PlayerMovementStateMachine.Player.Animator.SetBool(animationHash, false);
        }

        protected void SetSpeedAnimation(int animationHash, float value, float dampTime)
        {
            PlayerMovementStateMachine.Player.Animator.SetFloat(animationHash, value, dampTime, UnityEngine.Time.deltaTime);
        }
        
        protected void SetSpeedAnimation(int animationHash, float value)
        {
            PlayerMovementStateMachine.Player.Animator.SetFloat(animationHash, value);
        }
        
        protected virtual void AddInputActionsCallbacks()
        {
            PlayerMovementStateMachine.Player.Input.PlayerActions.WalkToggle.started += OnWalkToggleStarted;

            PlayerMovementStateMachine.Player.Input.PlayerActions.Look.started += OnMouseMovementStarted;

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.performed += OnMovementPerformed;
            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled += OnMovementCanceled;
        }

        protected virtual void RemoveInputActionsCallbacks()
        {
            PlayerMovementStateMachine.Player.Input.PlayerActions.WalkToggle.started -= OnWalkToggleStarted;

            PlayerMovementStateMachine.Player.Input.PlayerActions.Look.started -= OnMouseMovementStarted;

            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.performed -= OnMovementPerformed;
            PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.canceled -= OnMovementCanceled;
            
           
        }

        protected virtual void OnWalkToggleStarted(InputAction.CallbackContext context)
        {
            PlayerMovementStateMachine.ReusableData.ShouldWalk = !PlayerMovementStateMachine.ReusableData.ShouldWalk;
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
        
        private void ReadMovementInput()
        {
            PlayerMovementStateMachine.ReusableData.MovementInput = PlayerMovementStateMachine.Player.Input.PlayerActions.Movement.ReadValue<Vector2>();
        }

        protected void MoveAccordingCameraPosition()
        {
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero || PlayerMovementStateMachine.ReusableData.MovementSpeedModifier == 0f)
            {
                return;
            }
            Vector3 movementDirection = GetMovementInputDirection();
            float targetRotationYAngle = RotateTowardsTargetRotation(movementDirection);
            Vector3 targetRotationDirection = GetTargetRotationDirection(targetRotationYAngle);
            float movementSpeed = GetSmoothMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerMovementStateMachine.Player.Rigidbody.AddForce(targetRotationDirection * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }
        protected void MoveWithAimLocked()
        {
            if (PlayerMovementStateMachine.ReusableData.MovementInput == Vector2.zero || PlayerMovementStateMachine.ReusableData.MovementSpeedModifier == 0f)
            {
                return;
            }
            
            Vector3 movementDirection = GetMovementInputDirection();
            var forward = PlayerMovementStateMachine.Player.MainCameraTransform.forward;
            var right = PlayerMovementStateMachine.Player.MainCameraTransform.right;

            forward.y = 0;
            right.y = 0;
            forward.Normalize();
            right.Normalize();
            
            LockToAim();

            var desiredMovement = forward * movementDirection.z + right * movementDirection.x;
            float movementSpeed = GetSmoothMovementSpeed();
            Vector3 currentPlayerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerMovementStateMachine.Player.Rigidbody.AddForce(desiredMovement * movementSpeed - currentPlayerHorizontalVelocity, ForceMode.VelocityChange);
        }

        protected void LockToAim()
        {
            Vector3 worldAimTarget = PlayerMovementStateMachine.ReusableData.MouseWorldPosition;
            var transform = PlayerMovementStateMachine.Player.transform;
            worldAimTarget.y = transform.position.y;
            Vector3 aimDirection = (worldAimTarget - transform.position).normalized;

            PlayerMovementStateMachine.Player.transform.forward = Vector3.Lerp(PlayerMovementStateMachine.Player.transform.forward, aimDirection, Time.deltaTime * 20);
        }

        protected float GetSmoothMovementSpeed()
        {
            PlayerMovementStateMachine.ReusableData.SmoothModifier += Time.deltaTime;

            float movementSpeed = GetMovementSpeed();

            movementSpeed = Mathf.Lerp(0, GetMovementSpeed(), PlayerMovementStateMachine.ReusableData.SmoothModifier);

            return movementSpeed;
        }


        protected Vector3 GetMovementInputDirection()
        {
            return new Vector3(PlayerMovementStateMachine.ReusableData.MovementInput.x, 0f, PlayerMovementStateMachine.ReusableData.MovementInput.y);
        }

        private float RotateTowardsTargetRotation(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            RotateTowardsTargetRotation();

            return directionAngle;
        }
        
        private float Rotate(Vector3 direction)
        {
            float directionAngle = UpdateTargetRotation(direction);

            return directionAngle;
        }

        protected float UpdateTargetRotation(Vector3 direction, bool shouldConsiderCameraRotation = true)
        {
            float directionAngle = GetDirectionAngle(direction);

            if (shouldConsiderCameraRotation)
            {
                directionAngle = AddCameraRotationToAngle(directionAngle);
            }

            if (directionAngle != PlayerMovementStateMachine.ReusableData.CurrentTargetRotation.y)
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
            angle += PlayerMovementStateMachine.Player.MainCameraTransform.eulerAngles.y;

            if (angle > 360f)
            {
                angle -= 360f;
            }

            return angle;
        }

        private void UpdateTargetRotationData(float targetAngle)
        {
            PlayerMovementStateMachine.ReusableData.CurrentTargetRotation.y = targetAngle;

            PlayerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y = 0f;
        }

        protected void RotateTowardsTargetRotation()
        {
            float currentYAngle = PlayerMovementStateMachine.Player.Rigidbody.rotation.eulerAngles.y;

            if (currentYAngle == PlayerMovementStateMachine.ReusableData.CurrentTargetRotation.y)
            {
                return;
            }

            float smoothedYAngle = Mathf.SmoothDampAngle(currentYAngle, PlayerMovementStateMachine.ReusableData.CurrentTargetRotation.y, ref PlayerMovementStateMachine.ReusableData.DampedTargetRotationCurrentVelocity.y, PlayerMovementStateMachine.ReusableData.TimeToReachTargetRotation.y - PlayerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y);

            PlayerMovementStateMachine.ReusableData.DampedTargetRotationPassedTime.y += Time.deltaTime;

            Quaternion targetRotation = Quaternion.Euler(0f, smoothedYAngle, 0f);

            PlayerMovementStateMachine.Player.Rigidbody.MoveRotation(targetRotation);
        }

        protected Vector3 GetTargetRotationDirection(float targetRotationAngle)
        {
            return Quaternion.Euler(0f, targetRotationAngle, 0f) * Vector3.forward;
        }

        protected float GetMovementSpeed(bool shouldConsiderSlopes = true)
        {
            float movementSpeed = GroundedData.BaseSpeed * PlayerMovementStateMachine.ReusableData.MovementSpeedModifier;

            if (shouldConsiderSlopes)
            {
                movementSpeed *= PlayerMovementStateMachine.ReusableData.MovementOnSlopesSpeedModifier;
            }

            return movementSpeed;
        }

        protected Vector3 GetPlayerHorizontalVelocity()
        {
            Vector3 playerHorizontalVelocity = PlayerMovementStateMachine.Player.Rigidbody.velocity;

            playerHorizontalVelocity.y = 0f;

            return playerHorizontalVelocity;
        }

        protected Vector3 GetPlayerVerticalVelocity()
        {
            return new Vector3(0f, PlayerMovementStateMachine.Player.Rigidbody.velocity.y, 0f);
        }

        protected virtual void OnContactWithGround(Collider collider)
        {
        }

        protected virtual void OnContactWithGroundExited(Collider collider)
        {
        }


        protected void ResetVelocity()
        {
            PlayerMovementStateMachine.Player.Rigidbody.velocity = Vector3.zero;
        }

        protected void ResetVerticalVelocity()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerMovementStateMachine.Player.Rigidbody.velocity = playerHorizontalVelocity;
        }

        protected void DecelerateHorizontally()
        {
            Vector3 playerHorizontalVelocity = GetPlayerHorizontalVelocity();

            PlayerMovementStateMachine.Player.Rigidbody.AddForce(-playerHorizontalVelocity * PlayerMovementStateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void DecelerateVertically()
        {
            Vector3 playerVerticalVelocity = GetPlayerVerticalVelocity();

            PlayerMovementStateMachine.Player.Rigidbody.AddForce(-playerVerticalVelocity * PlayerMovementStateMachine.ReusableData.MovementDecelerationForce, ForceMode.Acceleration);
        }

        protected void RemoveMovementDebuff(IBonus bonus)
        {
            PlayerStateMachine.Player.BonusesController.RemoveBonus(bonus);
        }

        protected void AddMovementDebuff(IBonus bonus)
        {
            PlayerStateMachine.Player.BonusesController.AddBonus(bonus);
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