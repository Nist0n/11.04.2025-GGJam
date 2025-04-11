using System;
using UnityEngine;

namespace Environment
{
    public class TrapButton : MonoBehaviour
    {
        [SerializeField] private Trap trap;

        [SerializeField] private float timeToActivateButton;
        
        private bool _isButtonActive = true;

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
            if (other.CompareTag("Player") && _isButtonActive)
            {
                _animator.Play("Push");
                _isButtonActive = false;
                _timer = 0;
                trap.OnTrapButtonPush();
            }
        }

        private void ActivateButton()
        {
            if (_timer < timeToActivateButton && !_isButtonActive)
            {
                _timer += Time.deltaTime;
            }
            if (_timer > timeToActivateButton && !_isButtonActive)
            {
                _isButtonActive = true;
                _animator.Play("UnPush");
            }
        }
    }
}
