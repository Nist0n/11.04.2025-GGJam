using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float jumpForce;
        
        private const float Gravity = -9.81f;
        
        private CharacterController _characterController;
        private Vector3 _velocity;
        private Transform _cameraTransform;
        private bool _grounded;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _dashAction;

        private Vector2 _rotation;
        
        private float _rotationX;
        
        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            
            _cameraTransform = Camera.main.transform;
            
            _moveAction = InputSystem.actions.FindAction("Move");
            _lookAction = InputSystem.actions.FindAction("Look");
            _jumpAction = InputSystem.actions.FindAction("Jump");
            _sprintAction = InputSystem.actions.FindAction("Sprint");
            _dashAction = InputSystem.actions.FindAction("Dash");
        }

        private void Update()
        {
            Look();
            Move();
        }

        private void Look()
        {
            Vector2 lookValue = _lookAction.ReadValue<Vector2>() * (mouseSensitivity * Time.deltaTime);

            _rotationX -= lookValue.y;
            _rotationX = Mathf.Clamp(_rotationX, -90f, 90f);

            _cameraTransform.localRotation = Quaternion.Euler(_rotationX, 0f, 0f);
            
            transform.Rotate(Vector3.up, lookValue.x);
        }

        private void Move()
        {
            Vector2 moveValue = _moveAction.ReadValue<Vector2>();
            
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            
            Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;
            _characterController.Move(move * (moveSpeed * Time.deltaTime));
            
            if (_jumpAction.IsPressed() && _characterController.isGrounded)
            {
                _velocity.y = jumpForce;
            }
            
            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);
        }
    }
}