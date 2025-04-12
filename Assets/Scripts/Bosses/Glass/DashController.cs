using UnityEngine;

namespace Bosses.Glass
{
    public class BossDashController {
        private Transform _bossTransform;
        private float _dashSpeed;
        private float _dashDuration;
        private float _prepareTime = 1f;
        private float _stopDistance = 3f; // Дистанция до стены для остановки
        private LayerMask _wallMask;

        public DashState CurrentState = DashState.Ready;
        private float _stateTimer;
        private Vector3 _dashDirection;
        private float _cooldownTimer = 4f;

        public BossDashController(Transform boss, LayerMask wallLayer, float dashSpeed, float dashDuration) {
            _bossTransform = boss;
            _wallMask = wallLayer;
            _dashSpeed = dashSpeed;
            _dashDuration = dashDuration;
        }

        public void StartDash(Vector3 targetPosition) {
            if (CurrentState != DashState.Ready) return;
            
            _dashDirection = (targetPosition - _bossTransform.position).normalized;
            _dashDirection.y = 0; // Игнорируем вертикальную составляющую
            CurrentState = DashState.Preparing;
            _stateTimer = _prepareTime;
            
            // Поворачиваем босса к цели
            _bossTransform.rotation = Quaternion.LookRotation(_dashDirection);
        }

        public void UpdateDash() {
            switch (CurrentState) {
                case DashState.Preparing:
                    _stateTimer -= Time.deltaTime;
                    if (_stateTimer <= 0) {
                        CurrentState = DashState.Dashing;
                        _stateTimer = _dashDuration;
                    }
                    break;
                    
                case DashState.Dashing:
                    DashMovement();
                    break;
                    
                case DashState.Cooldown:
                    _stateTimer -= Time.deltaTime;
                    if (_stateTimer <= 0) {
                        CurrentState = DashState.Ready;
                    }
                    break;
            }
        }

        private void DashMovement() {
            float moveDistance = _dashSpeed * Time.deltaTime;
            
            // Проверяем столкновение со стеной
            if (Physics.Raycast(_bossTransform.position, _dashDirection, 
                              out RaycastHit hit, moveDistance + _stopDistance, _wallMask)) {
                // Останавливаемся перед стеной
                _bossTransform.position = hit.point - _dashDirection * _stopDistance;
                CurrentState = DashState.Cooldown;
                _stateTimer = _cooldownTimer; // Время "остывания"
                return;
            }
            
            // Двигаемся
            _bossTransform.position += _dashDirection * moveDistance;
            
            // Завершаем рывок по таймеру
            _stateTimer -= Time.deltaTime;
            if (_stateTimer <= 0) {
                CurrentState = DashState.Cooldown;
                _stateTimer = _cooldownTimer;
            }
        }

        public bool IsDashing() {
            return CurrentState != DashState.Ready;
        }
    }

    public enum DashState
    {
        Ready,
        Preparing,
        Dashing,
        Cooldown
    }
}