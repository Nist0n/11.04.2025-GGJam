using System.Collections;
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
            StartCoroutine(PlayBattleMusic());
            
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
            
            Vector3 direction = (Player.transform.position - transform.position).normalized;
            direction.y = 0; // (Опционально, если вращение должно быть только по горизонтали)
            
            // Вычисляем угол поворота (в градусах) + смещение
            float targetAngle = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg + 90;
            
            // Плавный поворот через Quaternion
            Quaternion targetRotation = Quaternion.Euler(0f, targetAngle, 0f);
            Quaternion newRotation = Quaternion.Slerp(Rb.rotation, targetRotation, 5 * Time.fixedDeltaTime);
            
            Rb.MoveRotation(newRotation); 
            
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
            GameEvents.BossDamaged?.Invoke();
            Health -= 1;
            AudioManager.instance.PlaySfx("ChestHit");
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

        private IEnumerator PlayBattleMusic()
        {
            AudioManager.instance.PlayMusic("ChestOpening");
            yield return new WaitForSeconds(1);
            AudioManager.instance.PlaySfx("Replica");
            float clipLength = AudioManager.instance.GetMusicClipLength("ChestOpening");
            yield return new WaitForSeconds(clipLength);
            AudioManager.instance.PlayMusic("ChestLoop");
        }
    }
}