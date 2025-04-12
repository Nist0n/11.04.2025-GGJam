using UnityEngine;

namespace Bosses.Chest.States
{
    public class FollowingState : BossState
    {
        public override void Enter()
        {
            //Core.BossAnimator.SetBool("IsMoving", true);
        }
    
        public override void Do()
        {
            Debug.Log(Core.IsGrounded());
            if (Core.IsGrounded())
            {
                JumpTowardsPlayer();
                IsComplete = true;
            }
        }
        
        public override void Exit()
        {
            //Core.BossAnimator.SetBool("IsMoving", false);
        }
        
        private void JumpTowardsPlayer()
        {
            Vector3 direction = (Core.Player.transform.position - Core.transform.position).normalized;
            direction.y = 0;
            
            Core.Rb.AddForce(direction * Core.Speed + Vector3.up * Core.JumpForce, ForceMode.Impulse);
            
            //Core.BossAnimator.SetTrigger("Jump");
        }
    }
}