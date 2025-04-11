using System;
using UnityEngine;

namespace Environment
{
    public class TrapButton : MonoBehaviour
    {
        [SerializeField] private Trap trap;

        private bool _isButtonActived = true;

        private float _timer;
        
        private float _timeToActivateButton = 10f;
        
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
            if (other.CompareTag("Player") && _isButtonActived)
            {
                _animator.Play("Push");
                _isButtonActived = false;
                _timer = 0;
                trap.OnTrapButtonPush();
            }
        }

        private void ActivateButton()
        {
            if (_timer < _timeToActivateButton && !_isButtonActived)
            {
                _timer += Time.deltaTime;
            }
            if (_timer > _timeToActivateButton && !_isButtonActived)
            {
                _isButtonActived = true;
                _animator.Play("UnPush");
            }
        }
    }
}
