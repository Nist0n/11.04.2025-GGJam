using System;
using System.Collections;
using Static_Classes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour
    {
        [SerializeField] private float walkSpeed;
        [SerializeField] private float mouseSensitivity;
        [SerializeField] private float jumpForce;
        [SerializeField] private float sprintMultiplier;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashTime;
        [SerializeField] private float dashCooldown;

        private const float Gravity = -9.81f;
        
        private CharacterController _characterController;
        private Vector3 _velocity;
        private Transform _cameraTransform;
        private bool _grounded;
        private bool _dashing;
        private bool _isGameLost;
        
        private InputAction _moveAction;
        private InputAction _lookAction;
        private InputAction _jumpAction;
        private InputAction _sprintAction;
        private InputAction _dashAction;
        
        private float _rotationX;
        private float _moveSpeed;

        private void SetOnPlayerDeath() => _isGameLost = true;
        
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
            if (_isGameLost)
            {
                return;
            }
            
            Look();
            Move();
        }

        private void OnDisable()
        {
            GameEvents.PlayerDeath -= SetOnPlayerDeath;
        }

        private void OnEnable()
        {
            GameEvents.PlayerDeath += SetOnPlayerDeath;
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
            if (_sprintAction.IsPressed() && moveValue is { y: > 0, x: 0 })
            {
                _moveSpeed = sprintMultiplier * walkSpeed;
            }
            else
            {
                _moveSpeed = walkSpeed;
            }
            
            if (_characterController.isGrounded && _velocity.y < 0)
            {
                _velocity.y = -2f;
            }
            
            Vector3 move = transform.right * moveValue.x + transform.forward * moveValue.y;
            
            _characterController.Move(move * (_moveSpeed * Time.deltaTime));
            
            if (_jumpAction.IsPressed() && _characterController.isGrounded)
            {
                _velocity.y = jumpForce;
            }
            
            _velocity.y += Gravity * Time.deltaTime;
            _characterController.Move(_velocity * Time.deltaTime);

            if (_dashAction.IsPressed() && !_dashing)
            {
                StartCoroutine(Dash());
            }

            IEnumerator Dash()
            {
                _dashing = true;
                float startTime = Time.time;

                while (Time.time < startTime + dashTime)
                {
                    Vector2 moveVector = _moveAction.ReadValue<Vector2>();
                    Vector3 direction = new Vector3(moveVector.x, 0f, moveVector.y).normalized;
                    if (direction.magnitude >= 0.1f)
                    {
                        float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + _cameraTransform.eulerAngles.y;
                        Vector3 moveDir = Quaternion.Euler(0f, targetAngle, 0f) * Vector3.forward;
                        
                        _characterController.Move(moveDir * (dashSpeed * Time.deltaTime));
                    }
                    
                    yield return null;
                }
                
                yield return new WaitForSeconds(dashCooldown);
                _dashing = false;
            }
        }
    }
}