using Bosses.Chest.States;
using Settings.Audio;
using Static_Classes;
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
        private bool _isGameLost = false;
        
        public IdleState Idle;
        public FollowingState Following;
        public DeathState Death;
        public AttackingState Attacking;
        public SwitchingPhaseState SwitchingPhase;
        
        private void Start()
        {
            Player = GameObject.FindGameObjectWithTag("Player");
            
            if (PlayerPrefs.GetInt("SecondPhaseStart") == 1)
            {
                Health = MaxHealth / 2;
                StartSecondPhase();
            }
            else
            {
                Health = MaxHealth;
            }
            SetupInstances();
            Set(Idle);
            SetNextAttackTime();
        }
        
        private void Update()
        {
            if (_isGameLost)
            {
                Set(Idle);
                return;
            }
            
            Health = Mathf.Clamp(Health, 0, MaxHealth);
            
            CheckNextAttackTimer();

            if (Health <= MaxHealth / 2 && BossPhase == Phase.First)
            {
                StartSecondPhase();
            }
            
            if (IsSwitchingPhase)
            {
                Set(SwitchingPhase);
                State.DoBranch();
                return;
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

        private void StartSecondPhase()
        {
            AudioManager.instance.PlaySfx("ChestBurp");
            IsSwitchingPhase = true;
            BossPhase = Phase.Second;
            GameEvents.SecondPhaseAchieved?.Invoke();
            Speed += 3;
            MinCoinsToDrop = 5;
            MaxCoinsToDrop = 9;
            idleTime = 1;
            minAttackDelay = 3;
            maxAttackDelay = 6;
        }
        
        private void SetNextAttackTime()
        {
            _nextAttackTime = Random.Range(minAttackDelay, maxAttackDelay);
            _nextAttackTimer = 0;
        }

        private void CheckNextAttackTimer()
        {
            if (_nextAttackTimer >= _nextAttackTime && !IsAttacking && IsGrounded())
            {
                IsAttacking = true;
                SetNextAttackTime();
            }
            else if (!IsAttacking)
            {
                _nextAttackTimer += Time.deltaTime;
            }
        }
        
        public void ReceiveDamage()
        {
            Health -= 2;
            AudioManager.instance.PlaySfx("RandomChestSound1");
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Player") && !_isGameLost)
            {
                GameEvents.PlayerDeath?.Invoke();
                AudioManager.instance.PlaySfx("RandomChestSound2");
                _isGameLost = true;
            }
        }
    }
}