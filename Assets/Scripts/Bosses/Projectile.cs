using Static_Classes;
using UnityEngine;

namespace Bosses
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        
        private Vector3 _direction;
        private float _progress;

        private void Update()
        {
            transform.position += _direction * (projectileSpeed * Time.deltaTime);
        }

        public void SetDirection(Vector3 direction)
        {
            _direction = direction.normalized;
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("BossGlass") || other.gameObject.CompareTag("Projectile"))
            {
                return;
            }
            if (other.gameObject.CompareTag("Player"))
            {
                GameEvents.PlayerDeath?.Invoke();
            }
            Destroy(gameObject);
        }
    }
}