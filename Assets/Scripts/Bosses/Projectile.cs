using Static_Classes;
using UnityEngine;

namespace Bosses
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        
        private Vector3 _direction;
        private float _progress;
        
        private float _timer;

        private const float TimeToDeath = 5;

        private void Update()
        {
            transform.position += _direction * (projectileSpeed * Time.deltaTime);
            
            if (_timer <= TimeToDeath)
            {
                _timer += Time.deltaTime;
            }
            else
            {
                DestroyProjectile();
            }
        }
        
        private void DestroyProjectile()
        {
            Destroy(gameObject);
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }

        public void SetSpeed(float speed)
        {
            projectileSpeed = speed;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("BossGlass") || other.gameObject.CompareTag("Projectile"))
            {
                return;
            }
            // if (other.gameObject.CompareTag("Player"))
            // {
            //     GameEvents.PlayerDeath?.Invoke();
            // }
            DestroyProjectile();
        }
    }
}