using Settings.Audio;
using System.Collections;
using UnityEngine;

namespace Bosses.Chest.States
{
    public class DeathState : BossState
    {
        public override void Enter()
        {
            AudioManager.instance.PlaySfx("BossDied");
            Core.BossAnimator.Play("Death");
            DestroyChest();
        }
    
        public override void Do()
        {
            
        }
    
        public override void Exit()
        {
        }

        private IEnumerator DestroyChest()
        {
            yield return new WaitForSeconds(3f);
            Destroy(gameObject);
        }
    }
}
