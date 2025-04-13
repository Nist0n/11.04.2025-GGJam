using Settings.Audio;
using UnityEngine;

namespace Bosses.Chest.States
{
    public class DeathState : BossState
    {
        public override void Enter()
        {
            AudioManager.instance.PlaySfx("BossDied");
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
