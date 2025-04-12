using System;
using UnityEngine;
using UnityEngine.Serialization;

namespace Bosses.Chest
{
    public abstract class Core : MonoBehaviour
    {
        public Rigidbody Rb;
        
        public Animator BossAnimator;
        
        public float Speed = 5f;
        
        public float JumpForce = 10f;
        
        public LayerMask GroundLayer;
        
        public float GroundCheckDistance = 0.2f;
        
        public Transform GroundCheckPoint;
        
        public float Health;
        
        public float MaxHealth = 100f;
        
        public bool IsAttacking = false;
        
        public bool IsSwitchingPhase = false;
        
        public StateMachine Machine;
        
        public float IdleTimer;

        public Phase BossPhase;

        public GameObject Player;
    
        public BossState State => Machine.State;

        protected void Set(BossState newState, bool forceReset = false)
        {
            Machine.Set(newState, forceReset);
        }

        protected void SetupInstances()
        {
            Machine = new StateMachine();

            BossState[] allchildStates = GetComponentsInChildren<BossState>();
            foreach (BossState state in allchildStates)
            {
                state.SetCore(this);
            }
        }
        
        public bool IsGrounded()
        {
            return Physics.CheckSphere(
                GroundCheckPoint.position, 
                GroundCheckDistance, 
                GroundLayer, 
                QueryTriggerInteraction.Ignore
            );
        }
    }

    public enum Phase
    {
        First,
        Second
    }
}
