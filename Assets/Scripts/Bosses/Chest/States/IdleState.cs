using UnityEngine;

namespace Bosses.Chest.States
{
    public class IdleState : BossState
    {
        public override void Enter()
        {
            //Core.BossAnimator.SetBool("IsMoving", false);
        }
    
        public override void Do()
        {
            IsComplete = true;
        }
    
        public override void Exit()
        {
        
        }
    }
}