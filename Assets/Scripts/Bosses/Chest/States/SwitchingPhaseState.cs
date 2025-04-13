using Settings.Audio;
using UnityEngine;

namespace Bosses.Chest.States
{
    public class SwitchingPhaseState : BossState
    {
        [SerializeField] private float timeToPhase2 = 7f;

        [SerializeField] private GameObject from;
        

        private float _timerToShoot;

        private float _timerToPhase2;
        
        public override void Enter()
        {
            Core.BossAnimator.Play("SwitchingPhase");
            _timerToPhase2 = Time.deltaTime;
            _timerToShoot = Time.deltaTime;
        }
    
        public override void Do()
        {
            if (timeToPhase2 >= _timerToPhase2 - Time.deltaTime)
            {
                _timerToPhase2 += Time.deltaTime;
                _timerToShoot += Time.deltaTime;
                if (_timerToShoot - Time.deltaTime >= 1)
                {
                    ShootProjectile(Core.Player.transform.position);
                    _timerToShoot = 0;
                }
            }
            else
            {
                IsComplete = true;
                Core.IsSwitchingPhase = false;
            }
        }
    
        public override void Exit()
        {
        
        }
        
        private void ShootProjectile(Vector3 targetPosition)
        {
            AudioManager.instance.PlaySfx("ChestHit");
            
            Vector3 direction = (targetPosition - Core.transform.position).normalized;
            
            var projectile = Instantiate(
                Core.ProjectilePrefab, 
                from.transform.position, 
                Quaternion.identity);
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }
    }
}
