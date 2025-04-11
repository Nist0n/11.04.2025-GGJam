using System;
using UnityEngine;

namespace Environment
{
    public class TrapButton : MonoBehaviour
    {
        [SerializeField] private Trap trap;

        [SerializeField] private float timeToActivateButton;
        
        private bool _isButtonActivated = true;

        private float _timer;
        
        
        
        private Animator _animator;

        private void Start()
        {
            _animator = gameObject.GetComponent<Animator>();
        }

        private void Update()
        {
            ActivateButton();
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && _isButtonActivated)
            {
                _animator.Play("Push");
                _isButtonActivated = false;
                _timer = 0;
                trap.OnTrapButtonPush();
            }
        }

        private void ActivateButton()
        {
            if (_timer < timeToActivateButton && !_isButtonActivated)
            {
                _timer += Time.deltaTime;
            }
            if (_timer > timeToActivateButton && !_isButtonActivated)
            {
                _isButtonActivated = true;
                _animator.Play("UnPush");
            }
        }
    }
}
