using System.Collections;
using AI.BehaviourTree.Base;
using UnityEngine;

namespace Bosses.Glass.BehaviourTree
{
    public class TaskShoot : Node
    {
        private Transform _transform;
        private Transform _playerTransform;
        private float _attackInterval;
        private float _projectileCount;
        private float _waveCooldown;
        private GameObject _projectile;

        private const float ProjectileSpeed = 0.5f;

        private bool _canShootWave;
        private float _timer;

        private bool _canShootOnce;
        private float _waveTimer = 1;
        
        public TaskShoot(Transform transform, Transform playerTransform, float attackInterval, float projectileCount, float waveCooldown, GameObject projectile)
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
            if (_canShootWave)
            {
                // shot a wave of projectiles towards the player
                int i = 0;
                while (i < _projectileCount)
                {
                    Debug.Log(_canShootOnce);
                    if (_canShootOnce)
                    {
                        Debug.Log("Can shoot one projectile");
                        // shoot one projectile
                        var shot = Object.Instantiate(_projectile, _transform.position, Quaternion.identity, _transform);
                        Vector3 dir = _playerTransform.position - _transform.position;
                        shot.GetComponent<Rigidbody>().linearVelocity = dir * ProjectileSpeed;
                        i++;
                        _canShootOnce = false;
                    }
                    else
                    {
                        _waveTimer -= Time.deltaTime;
                        if (_waveTimer <= 0)
                        {
                            _waveTimer = _attackInterval;
                            _canShootOnce = true;
                        }
                    }
                }
                
                _canShootWave = false;
            }
            else
            {
                _timer -= Time.deltaTime;
                if (_timer <= 0)
                {
                    _timer = _waveCooldown;
                    _canShootWave = true;
                }
            }

            return _state;
        }
    }
}