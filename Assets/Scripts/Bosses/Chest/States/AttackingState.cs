using System.Collections;
using DG.Tweening;
using Items;
using UnityEngine;

namespace Bosses.Chest.States
{
    public class AttackingState : BossState
    {
        [SerializeField] private float attackDuration = 1.5f;
        
        [SerializeField] private float brakingDistance = 1.5f;
        
        private Vector3 _attackDirection;
        private float _attackStartTime;
        private bool _isAirborne;
        private Vector3 _predictedTarget;
        private int _coinsToSpawn;

        public override void Enter()
        {
            _coinsToSpawn = Random.Range(Core.MinCoinsToDrop, Core.MaxCoinsToDrop);
            //Core.BossAnimator.SetTrigger("PrepareAttack");
            _predictedTarget = Core.Player.transform.position;
            _attackStartTime = Time.time;
            _isAirborne = false;
        }
        
        public override void Do()
        {
            if (Time.time - _attackStartTime > 0.3f && !_isAirborne)
            {
                LaunchAttack();
                StartCoroutine(DropCoins());
                _isAirborne = true;
            }

            if (_isAirborne)
            {
                UpdateMovement();
                CheckForLanding();
            }
        }
        
        public override void Exit()
        {
            //Core.BossAnimator.ResetTrigger("Attack");
        }

        private void LaunchAttack()
        {
            //Core.BossAnimator.SetTrigger("Attack");

            Vector3 toTarget = _predictedTarget - Core.transform.position;
            float gravity = Physics.gravity.magnitude;
            float angle = 45f * Mathf.Deg2Rad;
            
            float initialSpeed = Mathf.Sqrt(gravity * toTarget.magnitude / Mathf.Sin(2 * angle));
            
            Vector3 velocity = new Vector3(
                toTarget.normalized.x * initialSpeed * Mathf.Cos(angle),
                initialSpeed * Mathf.Sin(angle) * 1.2f,
                toTarget.normalized.z * initialSpeed * Mathf.Cos(angle)
            );
            
            Core.Rb.linearVelocity = velocity;
        }

        private void UpdateMovement()
        {
            float distanceToTarget = Vector3.Distance(Core.transform.position, _predictedTarget);
            if (distanceToTarget < brakingDistance)
            {
                Core.Rb.linearVelocity *= 0.95f;
            }
        }

        private void CheckForLanding()
        {
            if (Vector3.Distance(Core.transform.position, _predictedTarget) < 0.5f || 
                Time.time - _attackStartTime > attackDuration)
            {
                Core.Rb.linearVelocity = Vector3.zero;
                IsComplete = true;
                Core.IsAttacking = false;
                Core.IdleTimer = 0;
            }
        }
        
        void OnDrawGizmos()
        {
            if (_isAirborne)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawSphere(_predictedTarget, 0.3f);
            }
        }

        private IEnumerator DropCoins()
        {
            yield return new WaitForSeconds(0.25f);
            
            YieldInstruction[] coinsSpawnAnimations = new YieldInstruction[_coinsToSpawn];
            Coin[] coins = new Coin[_coinsToSpawn];

            for (int i = 0; i < _coinsToSpawn; i++)
            {
                if (i != 0)
                {
                    yield return new WaitForSeconds(0.4f);
                }

                Vector3 point = Core.transform.position;
                point.y += 2;

                coins[i] = Instantiate(Core.CoinPrefab, point,
                    Quaternion.Euler(0, Random.Range(0, 360), 0));

                if (Core.BossPhase == Phase.Second)
                {
                    coins[i].ChooseTheCoinType();
                }

                coinsSpawnAnimations[i] = AnimateSpawnFor(coins[i]);
            }

            foreach (var spawnAnimation in coinsSpawnAnimations)
            {
                yield return spawnAnimation;
            }
        }

        private YieldInstruction AnimateSpawnFor(Coin coin)
        {
            Vector2 randomOffset = Random.insideUnitCircle;
            Vector3 offset = new Vector3(randomOffset.x + Random.Range(-4, 4), Random.Range(1, 5), randomOffset.y);
            Vector3 jumpPosition = coin.transform.position + offset * 3f;

            return coin.transform.DOJump(jumpPosition, 2, 1, 0.7f).SetEase(Ease.OutBounce).Play().WaitForCompletion();
        }
    }
}