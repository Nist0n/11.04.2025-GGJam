using UnityEngine;

namespace Bosses.Chest.States
{
    public class SwitchingPhaseState : BossState
    {
        [SerializeField] private float timeToPhase2 = 7f;

        private float _timerToShoot;

        private float _timerToPhase2;
        
        public override void Enter()
        {
            //Core.BossAnimator.SetTrigger("AttackPhase");
            _timerToPhase2 = Time.deltaTime;
            _timerToShoot = Time.deltaTime;
        }
    
        public override void Do()
        {
            if (timeToPhase2 >= _timerToPhase2 - Time.deltaTime)
            {
                _timerToPhase2 += Time.deltaTime;
                _timerToShoot += Time.deltaTime;
                Debug.Log(_timerToShoot - Time.deltaTime);
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
            Vector3 direction = (targetPosition - Core.transform.position).normalized;
            
            var projectile = Instantiate(
                Core.ProjectilePrefab, 
                Core.transform.position, 
                Quaternion.LookRotation(direction));
            projectile.GetComponent<Projectile>().SetDirection(direction);
        }
    }
}
