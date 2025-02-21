using System;
using GameEventsSystem;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        // Access motor and weapons manager
        private CharacterMotor _motor;
        private WeaponManager _weaponManager;

        private Vector2 _moveInput;

        private void Awake()
        {
            _motor = GetComponent<CharacterMotor>();
            _weaponManager = FindAnyObjectByType<WeaponManager>();
        }

        public void OnMove(InputAction.CallbackContext context)
        {
            _moveInput = context.ReadValue<Vector2>();
        }

        public void OnJump(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Motor jump
                _motor.Jump();
            }
        }

        public void OnLook(InputAction.CallbackContext context)
        {
            var deltaPointer = context.ReadValue<Vector2>();
            
            _motor.Look(deltaPointer);
        }

        public void OnFire(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weaponManager.Attack();
            }
        }

        public void OnNext(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                // Interrupt current weapon
                _weaponManager.InterruptAttack();
                // Switch
                _weaponManager.SwitchWeapon(1);
            }
        }

        public void OnPrevious(InputAction.CallbackContext context)
        {
            if (context.performed)
            {
                _weaponManager.InterruptAttack();
                _weaponManager.SwitchWeapon(0);
            }
        }

        private void Update()
        {
            var movementDirection = new Vector3(_moveInput.x, 0, _moveInput.y);
            _motor.SetMovementInput(movementDirection);
        }
    }
}
