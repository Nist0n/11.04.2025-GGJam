using UnityEngine;

namespace Bosses.Chest.States
{
    public class DeathState : BossState
    {
        public override void Enter()
        {
            Core.BossAnimator.Play("Death");
        }
    
        public override void Do()
        {
            
        }
    
        public override void Exit()
        {
        
        }
    }
}
