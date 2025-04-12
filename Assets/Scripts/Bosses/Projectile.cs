using System;
using Static_Classes;
using UnityEngine;

namespace Bosses
{
    public class Projectile : MonoBehaviour
    {
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Boss") || other.gameObject.CompareTag("Projectile"))
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