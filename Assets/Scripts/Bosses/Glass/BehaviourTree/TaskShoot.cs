using AI.BehaviourTree.Base;
using UnityEngine;

namespace Bosses.Glass.BehaviourTree
{
    public class TaskShoot : Node
    {
        private Transform _transform;
        private Transform _playerTransform;
        private float _attackInterval;
        private int _projectileCount;
        private float _waveCooldown;
        private GameObject _projectile;

        private float _nextWaveTime;
        private int _projectilesFiredInCurrentWave;
        private float _nextProjectileTime;
        
        public TaskShoot(Transform transform, Transform playerTransform, float attackInterval, int projectileCount, float waveCooldown, GameObject projectile)
        {
            _transform = transform;
            _playerTransform = playerTransform;
            _attackInterval = attackInterval;
            _projectileCount = projectileCount;
            _waveCooldown = waveCooldown;
            _projectile = projectile;
        }

        public override NodeState Evaluate()
        {
            _state = NodeState.Running;
            if (Time.time >= _nextWaveTime && _projectilesFiredInCurrentWave >= _projectileCount)
            {
                _projectilesFiredInCurrentWave = 0;
                _nextWaveTime = Time.time + _waveCooldown;
            }
            
            if (_projectilesFiredInCurrentWave < _projectileCount && Time.time >= _nextProjectileTime)
            {
                ShootProjectile(_playerTransform.position);
                _projectilesFiredInCurrentWave++;
                _nextProjectileTime = Time.time + _attackInterval;
            
                // Если волна завершена, устанавливаем время для следующей волны
                if (_projectilesFiredInCurrentWave >= _projectileCount)
                {
                    _nextWaveTime = Time.time + _waveCooldown;
                }
            }
            return _state;
        }
        
        private void ShootProjectile(Vector3 targetPosition)
        {
            // Рассчитываем направление к игроку
            Vector3 direction = (targetPosition - _transform.position).normalized;
        
            // Создаем снаряд
            var projectile = Object.Instantiate(
                _projectile, 
                _transform.position, 
                Quaternion.LookRotation(direction));
            Projectile projectileScript = projectile.GetComponent<Projectile>();
            projectileScript.SetDirection(direction);
        }
    }
}