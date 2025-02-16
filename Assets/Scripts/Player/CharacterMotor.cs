using System;
using UnityEngine;
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
        
        private CharacterController _characterController;
        private Vector3 _currentVelocity;
        private bool _isWallRunning = false;
        private float _xRotation = 0f;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
        }

        public void SetMovementInput(Vector3 inputDirection)
        {
            var move = transform.TransformDirection(inputDirection) * walkSpeed;
            _currentVelocity.x = move.x;
            _currentVelocity.z = move.z;
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
            
            var cameraTransform = transform.GetChild(0);
            _xRotation -= lookDelta.y * mouseSensitivityY;
            _xRotation = Mathf.Clamp(_xRotation, -85f, 90f);
            var targetRotation = transform.eulerAngles;
            targetRotation.x = _xRotation;
            cameraTransform.eulerAngles = targetRotation;

        }

        private void Update()
        {
            if (!_isWallRunning)
            {
                _currentVelocity.y += gravity * Time.deltaTime; // gravity value is negative
            }
            _characterController.Move(_currentVelocity * Time.deltaTime);
        }
    }
}
