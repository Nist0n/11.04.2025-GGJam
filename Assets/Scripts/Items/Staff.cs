using System;
using Bosses;
using Settings.Audio;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Items
{
    public class Staff : MonoBehaviour
    {
        [SerializeField] private Transform fromObj;

        [SerializeField] private GameObject projectile;

        [SerializeField] private float timeToReset;

        [SerializeField] private float projectileSpeed;

        [SerializeField] private GameObject lightPoint;
        
        private float _timer;

        private bool _isSkillActive = true;

        private Camera _camera;

        private InputAction _attackAction;
        
        private void Start()
        {
            _camera = Camera.main;

            _attackAction = InputSystem.actions.FindAction("Attack");
        }

        private void Update()
        {
            lightPoint.SetActive(_isSkillActive);
            SkillCooldown();
            OnLeftMouseButtonClick();
        }

        private void OnLeftMouseButtonClick()
        {
            if (_attackAction.IsPressed() && _isSkillActive) // Left button
            {
                AudioManager.instance.PlaySfx("MagicShot");
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
