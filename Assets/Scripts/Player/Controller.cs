using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float mouseSensitivity;
        
        private const float Gravity = -9.81f;
        
        private CharacterController _characterController;
        private Vector3 _velocity;
        private Transform _cameraTransform;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _dashAction;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
            _cameraTransform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            _cameraTransform.parent = transform;
            
            _moveAction = InputSystem.actions.FindAction("Move");
            _lookAction = InputSystem.actions.FindAction("Look");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _sprintAction = InputSystem.actions.FindAction("Sprint");
            _dashAction = InputSystem.actions.FindAction("Dash");
        }

        private void Update()
        {
            HandleInput();
            ApplyGravity();
        }

        private void HandleInput()
        {
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            Vector2 lookValue = _lookAction.ReadValue<Vector2>() * mouseSensitivity;
            
            transform.Rotate(Vector3.up * lookValue.x);
            _cameraTransform.Rotate(Vector3.left * lookValue.y);
            
            Vector3 move = moveValue.x * transform.right + moveValue.y * transform.forward;
            _characterController.Move(move * (moveSpeed * Time.deltaTime));
        }
        
        private void ApplyGravity()
        {
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}