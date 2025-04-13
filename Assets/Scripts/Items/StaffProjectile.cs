using System;
using System.Collections;
using Settings.Audio;
using UnityEngine;

namespace Items
{
    public class StaffProjectile : MonoBehaviour
    {
        [SerializeField] private float projectileSpeed;
        
        [SerializeField] private GameObject hitEffect;
        
        private Vector3 _direction;
        
        private float _progress;
        
        private float _timer;

        private const float TimeToDeath = 5;

        private GameObject _boss;

        private void Start()
        {
            if (GameObject.FindGameObjectWithTag("BossChest"))
            {
                _boss = GameObject.FindGameObjectWithTag("BossChest");
            }
            else
            {
                _boss = GameObject.FindGameObjectWithTag("BossGlass");
            }
        }

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
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("BossGlass") || other.CompareTag("BossChest"))
            {
                StartCoroutine(ShowHit());
            }
        }

        private IEnumerator ShowHit()
        {
            AudioManager.instance.PlaySfx("SkillHit");
            var temp = Instantiate(hitEffect, gameObject.transform.position, Quaternion.identity, _boss.transform);
            yield return new WaitForSeconds(1);
            Destroy(temp);
            DestroyProjectile();
        }
    }
}
