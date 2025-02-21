using System;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.TextCore.Text;

namespace Player
{
    public class CharacterMotor : MonoBehaviour
    {
        public float walkSpeed = 5f;
        public float jumpForce = 5f;
        public float gravity = -9.81f;
        public float mouseSensitivityX;
        public float mouseSensitivityY;
        public float movementSmoothingTime = 0.1f;
        public float lookSmoothingFactor = 0.1f;
        
        private CharacterController _characterController;
        private Vector3 _currentVelocity = Vector3.zero;
        private Vector3 _smoothVelocity = Vector3.zero;
        private bool _isWallRunning = false;
        private float _xRotation = 0f;
        private Quaternion _cameraTargetRotation = Quaternion.identity;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void SetMovementInput(Vector3 inputDirection)
        {
            // Compute the target horizontal velocity only.
            var targetHorizontalVelocity = transform.TransformDirection(inputDirection.normalized) * walkSpeed;
    
            // Keep the current vertical velocity intact.
            var currentVerticalVelocity = _currentVelocity.y;
    
            // Smooth the horizontal velocity only.
            var currentHorizontalVelocity = new Vector3(_currentVelocity.x, 0, _currentVelocity.z);
            currentHorizontalVelocity = Vector3.SmoothDamp(currentHorizontalVelocity, targetHorizontalVelocity, ref _smoothVelocity, movementSmoothingTime);
    
            // Combine the new horizontal velocity with the existing vertical velocity.
            _currentVelocity = currentHorizontalVelocity + new Vector3(0, currentVerticalVelocity, 0);
        }

        public void Jump()
        {
            if (_characterController.isGrounded)
            {
                _currentVelocity.y = jumpForce;
            }
        }

        public void Look(Vector2 lookDelta)
        {
            transform.Rotate(transform.up, lookDelta.x * mouseSensitivityX * Time.deltaTime);
            
            _xRotation -= lookDelta.y * mouseSensitivityY * Time.deltaTime;
            _xRotation = Mathf.Clamp(_xRotation, -85f, 90f);
            
            _cameraTargetRotation = Quaternion.Euler(_xRotation, 0f, 0f);
        }

        private void Update()
        {
            if (!_isWallRunning)
            {
                _currentVelocity.y += gravity * Time.deltaTime; // gravity value is negative
            }
            _characterController.Move(_currentVelocity * Time.deltaTime);
        }

        private void LateUpdate()
        {
            var cameraTransform = transform.GetChild(0);
            cameraTransform.localRotation = Quaternion.Slerp(cameraTransform.localRotation, _cameraTargetRotation, lookSmoothingFactor * Time.deltaTime);
        }
    }
}
