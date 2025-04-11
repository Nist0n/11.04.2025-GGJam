using UnityEngine;

namespace Bosses.Chest
{
    public abstract class BossState : MonoBehaviour
    {
        public bool IsComplete { get; protected set; }
        
        protected float StartTime;
        
        public StateMachine Machine;
        
        protected Core Core;
        
        public StateMachine Parent;
        
        public BossState State => Machine.State;
    
        protected void Set(BossState newState, bool forceReset = false)
        {
            Machine.Set(newState, forceReset);
        }

        public void SetCore(Core _core)
        {
            Machine = new StateMachine();
            Core = _core;
        }

        public virtual void Enter() {}
    
        public virtual void Do() {}
    
        public virtual void FixedDo() {}
    
        public virtual void Exit() {}

        public void DoBranch()
        {
            Do();
            State?.DoBranch();
        }
    
        public void FixedDoBranch()
        {
            FixedDo();
            State?.FixedDoBranch();
        }
    
        public void Initialise(StateMachine _parent)
        {
            Parent = _parent;
            IsComplete = false;
            StartTime = Time.time;
        }
    }
}
