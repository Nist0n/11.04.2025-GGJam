using System.Collections;
using DG.Tweening;
using Settings.Audio;
using UnityEngine;

namespace Environment
{
    public class Trap : MonoBehaviour
    {
        [SerializeField] private GameObject fireball;

        public void OnTrapButtonPush()
        {
            Vector3 spawnPos = new Vector3(transform.position.x, transform.position.y + 5, transform.position.z);
            GameObject projectile = Instantiate(fireball, spawnPos, Quaternion.identity);
            
            AudioManager.instance.PlaySfx("FireTrap");

            Vector3 newPos = projectile.transform.position;
            newPos.y += 15;

            projectile.transform.DOMove(newPos, 1.5f);
            StartCoroutine(DestroyProjectile(projectile));
        }

        private IEnumerator DestroyProjectile(GameObject projectile)
        {
            yield return new WaitForSeconds(3);
            if (projectile)
            {
                Destroy(projectile);
            }
        }
    }
}
