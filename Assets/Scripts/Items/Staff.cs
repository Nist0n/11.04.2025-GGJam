using System;
using Bosses;
using UnityEngine;

namespace Items
{
    public class Staff : MonoBehaviour
    {
        [SerializeField] private Transform fromObj;

        [SerializeField] private GameObject projectile;

        [SerializeField] private float timeToReset;

        [SerializeField] private float projectileSpeed;
        

        private float _timer;

        private bool _isSkillActive = true;

        private Camera _camera;

        private void Start()
        {
            _camera = Camera.main;
        }

        private void Update()
        {
            SkillCooldown();
            OnLeftMouseButtonClick();
        }

        private void OnLeftMouseButtonClick()
        {
            if (Input.GetMouseButtonUp(0) && _isSkillActive) // Left button
            {
                ShootProjectile();
                _timer = 0;
                _isSkillActive = false;
            }
        }

        private void SkillCooldown()
        {
            if (_timer < timeToReset && !_isSkillActive)
            {
                _timer += Time.deltaTime;
            }
            if (_timer > timeToReset && !_isSkillActive)
            {
                _isSkillActive = true;
            }
        }
        
        private void ShootProjectile()
        {
            Vector3 castDirection = _camera.transform.forward;
            castDirection.x -= 0.05f;
            
            var temp = Instantiate(
                this.projectile, 
                fromObj.transform.position, 
                Quaternion.LookRotation(castDirection));
            temp.GetComponent<StaffProjectile>().SetDirection(castDirection);
        }
    }
}
