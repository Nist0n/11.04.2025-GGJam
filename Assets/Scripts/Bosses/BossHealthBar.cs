using System;
using Static_Classes;
using UnityEngine;
using UnityEngine.UI;

namespace Bosses
{
    public class BossHealthBar : MonoBehaviour
    {
        [SerializeField] private Slider healthSlider;
        [SerializeField] private int maxHealth;

        private int _health;

        private void Start()
        {
            _health = maxHealth;
            if (PlayerPrefs.GetInt("SecondPhaseStart") == 1)
            {
                _health = maxHealth / 2;
            }

            healthSlider.maxValue = maxHealth;
            healthSlider.value = _health;
        }
        
        private void OnEnable()
        {
            GameEvents.BossDamaged += OnReceiveDamage;
        }

        private void OnDisable()
        {
            GameEvents.BossDamaged -= OnReceiveDamage;
        }

        private void OnReceiveDamage()
        {
            _health--;
            healthSlider.value--;
            if (_health <= 0)
            {
                gameObject.SetActive(false);
            }
        }
    }
}