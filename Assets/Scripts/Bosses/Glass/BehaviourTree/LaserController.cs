using DG.Tweening;
using Static_Classes;
using UnityEngine;
using UnityEngine.AI;

namespace Bosses.Glass.BehaviourTree
{
    public class LaserController
    {
        private Transform _bossTransform;
        private float _rotationSpeed;
        private float _prepareTime = 1f;
        private float _cooldownTime = 10f;
        private float _groundLevel = 0.2f;
        private float _descentSpeed = 0.05f;

        private float _currentRotation;

        public LaserState CurrentState = LaserState.Ready;
        private float _stateTimer;
        
        private LineRenderer _lineRenderer;
        private NavMeshAgent _agent;

        private float _laserLength = 50f;

        private AudioSource _laserSound;

        private float _descentDuration = 1.2f;
        
        public LaserController(Transform bossTransform, float rotationSpeed, LineRenderer lineRenderer, NavMeshAgent agent, AudioSource laserSound)
        {
            _bossTransform = bossTransform;
            _rotationSpeed = rotationSpeed;
            _lineRenderer = lineRenderer;
            _lineRenderer.enabled = false;
            _agent = agent;
            _laserSound = laserSound;
        }

        public void StartLasering()
        {
            if (CurrentState != LaserState.Ready)
            {
                return;
            }

            CurrentState = LaserState.Preparing;
            _stateTimer = _prepareTime;
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
            DOTween.To(() => _agent.baseOffset, x => _agent.baseOffset = x, 0, _descentDuration);
            
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
            
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                CurrentState = LaserState.Lasering;
                _lineRenderer.enabled = true;
                _currentRotation = 0f;
                float r = Random.Range(0.5f, 2f);
                _stateTimer = 360f / _rotationSpeed + r;
            }
        }

        private void Laser()
        {
            _laserSound.enabled = true;
            // lasering
            _agent.isStopped = true;
            float rotationAmount = _rotationSpeed * Time.deltaTime;
            _bossTransform.Rotate(Vector3.up, rotationAmount);
            _currentRotation += rotationAmount;
            UpdateLaser();
            
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0)
            {
                CurrentState = LaserState.Cooldown;
                _stateTimer = _cooldownTime;
                _lineRenderer.enabled = false;
                _laserSound.enabled = false;
                _agent.isStopped = false;
                DOTween.To(() => _agent.baseOffset, x => _agent.baseOffset = x, 1.75f, _descentDuration);
            }
        }

        private void UpdateLaser()
        {
            if (!_lineRenderer.enabled) return;
            
            Vector3 laserStartPos = new Vector3(
                _bossTransform.position.x,
                _bossTransform.position.y + 0.3f,
                _bossTransform.position.z
            );
            Vector3 laserEnd = _bossTransform.position + _bossTransform.forward * _laserLength;

            if (Physics.Raycast(laserStartPos, _bossTransform.forward,
                    out RaycastHit hit, _laserLength))
            {
                // if (hit.collider.gameObject.layer == LayerMask.GetMask("Wall"))
                // {
                //     laserEnd = hit.point;
                // }
                // Debug.Log(hit.collider.gameObject.name);
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    GameEvents.PlayerDeath?.Invoke();
                }
            }

            
            _lineRenderer.SetPosition(0, laserStartPos);
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
        
        public bool IsLasering()
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