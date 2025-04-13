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
            healthSlider.maxValue = maxHealth;
            healthSlider.value = maxHealth;
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
        }
    }
}