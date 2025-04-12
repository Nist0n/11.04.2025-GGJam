using Bosses.Chest.States;
using UnityEngine;

namespace Bosses.Chest
{
    public class Chest : Core
    {
        [SerializeField] private float idleTime = 3f;
        [SerializeField] private float minAttackDelay = 7f;
        [SerializeField] private float maxAttackDelay = 15f;
        
        private float _nextAttackTime;
        private float _nextAttackTimer;
        
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
            SetNextAttackTime();
        }
        
        private void Update()
        {
            if (IsSwitchingPhase)
            {
                Set(SwitchingPhase);
                return;
            }
            
            if (_nextAttackTimer >= _nextAttackTime && !IsAttacking && IsGrounded())
            {
                IsAttacking = true;
                SetNextAttackTime();
            }
            else if (!IsAttacking)
            {
                _nextAttackTimer += Time.deltaTime;
            }
            
            if (State.IsComplete)
            {
                if (IdleTimer <= idleTime)
                {
                    IdleTimer += Time.deltaTime;
                    Set(Idle);
                }
                else if (IsAttacking)
                {
                    Set(Attacking);
                }
                else
                {
                    Set(Following);
                    IdleTimer = 0;
                }
            }
            
            if (Health <= 0)
            {
                Set(Death);
            }
            
            State.DoBranch();
        }
        
        private void FixedUpdate()
        {
            State.FixedDoBranch();
        }
        
        private void SetNextAttackTime()
        {
            _nextAttackTime = Random.Range(minAttackDelay, maxAttackDelay);
            _nextAttackTimer = 0;
        }
    }
}