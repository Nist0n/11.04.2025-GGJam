using System;
using UnityEngine;

namespace Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Controller : MonoBehaviour
    {
        [SerializeField] private float moveSpeed;
        [SerializeField] private float mouseSensitivity;
        
        private CharacterController _characterController;
        private Transform _cameraTransform;

        private void Start()
        {
            _characterController = GetComponent<CharacterController>();
            _cameraTransform = Camera.main.transform;
            _cameraTransform.position = new Vector3(transform.position.x, transform.position.y + 1.5f, transform.position.z);
            _cameraTransform.parent = transform;
        }

        private void Update()
        {
            HandleInput();
        }

        private void HandleInput()
        {
            float moveX = Input.GetAxis("Horizontal") * moveSpeed;
            float moveY = Input.GetAxis("Vertical") * moveSpeed;
            Vector3 movement = transform.right * moveX + transform.forward * moveY;
            
            _characterController.Move(movement * Time.deltaTime);
            float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity;
            float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity;

            transform.Rotate(Vector3.up * mouseX);
            _cameraTransform.Rotate(Vector3.left * mouseY);
        }
    }
}