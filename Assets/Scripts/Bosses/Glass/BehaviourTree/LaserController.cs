using UnityEngine;

namespace Bosses.Glass.BehaviourTree
{
    public class LaserController
    {
        private Transform _bossTransform;
        private float _rotationSpeed;
        private float _laserDuration = 5f;
        private float _prepareTime = 1.5f;
        private float _cooldownTime = 10f;
        private float _groundLevel = 0f;
        private float _descentSpeed = 3f;

        private float _currentRotation;
        private Vector3 _initialForward;

        public LaserState CurrentState = LaserState.Ready;
        private float _stateTimer;
        
        private LineRenderer _lineRenderer;

        public LaserController(Transform bossTransform, float rotationSpeed, LineRenderer lineRenderer)
        {
            _bossTransform = bossTransform;
            _rotationSpeed = rotationSpeed;
            _lineRenderer = lineRenderer;
            _lineRenderer.enabled = false;
        }

        public void StartLasering()
        {
            if (CurrentState != LaserState.Ready)
            {
                return;
            }

            CurrentState = LaserState.Preparing;
            _stateTimer = _prepareTime;
            _initialForward = _bossTransform.forward;
        }

        public void UpdateLasering()
        {
            switch (CurrentState)
            {
                case LaserState.Preparing:
                    HandlePreparation();
                    break;
                
                case LaserState.Lasering:
                    Laser();
                    break;
                case LaserState.Cooldown:
                    HandleCooldown();
                    break;
            }
        }

        private void HandlePreparation()
        {
            if (_bossTransform.position.y > _groundLevel + 0.1f)
            {
                _bossTransform.position -= Vector3.up * (_descentSpeed * Time.deltaTime);
                return;
            }

            _bossTransform.position = new Vector3(
                _bossTransform.position.x,
                _groundLevel,
                _bossTransform.position.z
            );
            
            // _bossTransform.forward = _initialForward;
            
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                CurrentState = LaserState.Lasering;
                _lineRenderer.enabled = true;
                _currentRotation = 0f;
            }
        }

        private void Laser()
        {
            // lasering
            float rotationAmount = _rotationSpeed * Time.deltaTime;
            _bossTransform.Rotate(Vector3.up, rotationAmount);
            _currentRotation += rotationAmount;
            UpdateLaser();

            if (_currentRotation >= 360f)
            {
                CurrentState = LaserState.Cooldown;
                _stateTimer = _cooldownTime;
                _lineRenderer.enabled = false;
            }
            
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                CurrentState = LaserState.Cooldown;
                _stateTimer = _cooldownTime;
            }
        }

        private void UpdateLaser()
        {
            if (!_lineRenderer.enabled) return;

            float laserLength = 50f;
            Vector3 laserEnd = _bossTransform.position + _bossTransform.forward * laserLength;

            if (Physics.Raycast(_bossTransform.position, _bossTransform.forward,
                    out RaycastHit hit, laserLength))
            {
                laserEnd = hit.point;
            }
            
            _lineRenderer.SetPosition(0, _bossTransform.position);
            _lineRenderer.SetPosition(1, laserEnd);
        }

        private void HandleCooldown()
        {
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                CurrentState = LaserState.Ready;
            }
        }
        
        public bool isLasering()
        {
            return CurrentState != LaserState.Ready;
        }
    }

    public enum LaserState
    {
        Ready,
        Preparing,
        Lasering,
        Cooldown
    }
}