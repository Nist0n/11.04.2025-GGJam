using System;
using UnityEngine;

namespace Bosses.Chest
{
    public abstract class Core : MonoBehaviour
    {
        public Animator BossAnimator;
        
        public StateMachine Machine;
        
        public float Speed;
        
        public float Health;
        
        public float MaxHealth;
        
        public bool IsAttacking = false;
        
        public bool IsSwitchingPhase = false;

        public Phase BossPhase;
    
        public BossState State => Machine.State;
    
        protected void Set(BossState newState, bool forceReset = false)
        {
            Machine.Set(newState, forceReset);
        }

        public void SetupInstances()
        {
            Machine = new StateMachine();

            BossState[] allchildStates = GetComponentsInChildren<BossState>();
            foreach (BossState state in allchildStates)
            {
                state.SetCore(this);
            }
        }
    }

    public enum Phase
    {
        First,
        Second
    }
}
