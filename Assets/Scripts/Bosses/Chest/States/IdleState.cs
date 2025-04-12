using Settings.Audio;
using UnityEngine;

namespace Bosses.Chest.States
{
    public class IdleState : BossState
    {
        public override void Enter()
        {
            int rand = Random.Range(1, 4);

            if (rand == 1) AudioManager.instance.PlaySfx("ChestBurp");
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