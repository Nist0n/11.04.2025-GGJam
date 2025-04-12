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
            GameObject projectile = Instantiate(fireball, transform);
            
            AudioManager.instance.PlaySfx("FireTrap");

            Vector3 newPos = projectile.transform.position;
            newPos.y += 10;

            projectile.transform.DOMove(newPos, 2);
        }
    }
}
