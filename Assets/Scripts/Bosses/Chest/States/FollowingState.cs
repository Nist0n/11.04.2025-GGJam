using Settings.Audio;
using UnityEngine;

namespace Bosses.Chest.States
{
    public class FollowingState : BossState
    {
        public override void Enter()
        {
            
        }
    
        public override void Do()
        {
            if (Core.IsGrounded())
            {
                JumpTowardsPlayer();
                IsComplete = true;
            }
        }
        
        public override void Exit()
        {
            
        }
        
        private void JumpTowardsPlayer()
        {
            AudioManager.instance.PlaySfx("RandomChestSound1");
            
            Vector3 direction = (Core.Player.transform.position - Core.transform.position).normalized;
            direction.y = 0;
            
            Core.Rb.AddForce(direction * Core.Speed + Vector3.up * Core.JumpForce, ForceMode.Impulse);
        }
    }
}