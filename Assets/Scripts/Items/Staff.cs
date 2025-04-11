using System;
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
        
        private void Update()
        {
            SkillCooldown();
            OnLeftMouseButtonClick();
        }

        private void OnLeftMouseButtonClick()
        {
            if (Input.GetMouseButtonUp(0) && _isSkillActive) // Left button
            {
                GameObject temp = Instantiate(projectile, fromObj.position, Quaternion.identity, gameObject.transform);
                temp.GetComponent<Rigidbody>().linearVelocity = -fromObj.forward * projectileSpeed;
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
    }
}
