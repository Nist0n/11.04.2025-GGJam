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

        public override void Enter()
        {
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
    }
}