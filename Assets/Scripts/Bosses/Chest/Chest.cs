using Bosses.Chest.States;
using UnityEngine;

namespace Bosses.Chest
{
    public class Chest : Core
    {
        [SerializeField] private float idleTime;

        private float _idleTimer;
        
        public IdleState Idle;
        public FollowingState Following;
        public DeathState Death;
        public AttackingState Attacking;
        public SwitchingPhaseState SwitchingPhase;
        
        private void Start()
        {
            Health = MaxHealth;
            SetupInstances();
            Set(Idle);
        }
        
        private void Update()
        {
            Health = Mathf.Clamp(Health, 0, MaxHealth);

            if (IsSwitchingPhase)
            {
                Set(SwitchingPhase);
            }
            
            if (State.IsComplete)
            {
                if (IsAttacking)
                {
                    Set(Attacking);
                }
                else
                {
                    if (_idleTimer <= idleTime)
                    {
                        _idleTimer += Time.deltaTime;
                        Set(Idle);
                    }
                    else
                    {
                        Set(Following);
                        _idleTimer = 0;
                    }
                }
            }

            if (Health <= 0)
            {
                Set(Death);
                //Invoke(nameof(KillBoss), 1f);
            }
            
            State.DoBranch();
        }
        
        private void FixedUpdate()
        {
            State.FixedDoBranch();
        }
    }
}
