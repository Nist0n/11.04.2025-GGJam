using UnityEngine;

namespace Bosses.Glass
{
    public class BossDashController {
        private Transform _bossTransform;
        private float dashSpeed = 80f;
        private float dashDuration = 0.3f;
        private float prepareTime = 1f;
        private float stopDistance = 3f; // Дистанция до стены для остановки
        private LayerMask wallMask;

        public DashState currentState = DashState.Ready;
        private float stateTimer;
        private Vector3 dashDirection;

        public BossDashController(Transform boss, LayerMask wallLayer) {
            _bossTransform = boss;
            wallMask = wallLayer;
        }

        public void StartDash(Vector3 targetPosition) {
            if (currentState != DashState.Ready) return;
            
            dashDirection = (targetPosition - _bossTransform.position).normalized;
            dashDirection.y = 0; // Игнорируем вертикальную составляющую
            currentState = DashState.Preparing;
            stateTimer = prepareTime;
            
            // Поворачиваем босса к цели
            _bossTransform.rotation = Quaternion.LookRotation(dashDirection);
        }

        public void UpdateDash() {
            switch (currentState) {
                case DashState.Preparing:
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0) {
                        currentState = DashState.Dashing;
                        stateTimer = dashDuration;
                    }
                    break;
                    
                case DashState.Dashing:
                    DashMovement();
                    break;
                    
                case DashState.Cooldown:
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0) {
                        currentState = DashState.Ready;
                    }
                    break;
            }
        }

        private void DashMovement() {
            float moveDistance = dashSpeed * Time.deltaTime;
            
            // Проверяем столкновение со стеной
            if (Physics.Raycast(_bossTransform.position, dashDirection, 
                              out RaycastHit hit, moveDistance + stopDistance, wallMask)) {
                // Останавливаемся перед стеной
                _bossTransform.position = hit.point - dashDirection * stopDistance;
                currentState = DashState.Cooldown;
                stateTimer = 4f; // Время "остывания"
                return;
            }
            
            // Двигаемся
            _bossTransform.position += dashDirection * moveDistance;
            
            // Завершаем рывок по таймеру
            stateTimer -= Time.deltaTime;
            if (stateTimer <= 0) {
                currentState = DashState.Cooldown;
                stateTimer = 4f;
            }
        }

        public bool IsDashing() {
            return currentState != DashState.Ready;
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