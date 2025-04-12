using System.Collections.Generic;
using AI.BehaviourTree.Base;
using Bosses.Glass.BehaviourTree;
using Static_Classes;
using UnityEngine;
using UnityEngine.AI;

namespace Bosses.Glass
{
    public class GlassAI : AI.BehaviourTree.Base.BehaviourTree
    {
        [SerializeField] private float health;
        
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform targetTransform;
        [SerializeField] private GameObject projectile;
        [SerializeField] private float intervalBetweenShots;
        [SerializeField] private int projectileCount;
        [SerializeField] private float waveCooldown;
        [SerializeField] private float avoidDistance;
        [SerializeField] private float dashSpeed;
        [SerializeField] private float dashDuration;
        
        private Node _rootNode;

        private NavMeshAgent _agent;
        private Transform _transform;

        private int _currentPhase = 1;
        
        private void Start()
        {
            _rootNode = SetupTree();
        }

        protected override Node SetupTree()
        {
            _agent = GetComponent<NavMeshAgent>();
            _transform = transform;
            
            
            Node root = new Selector();

            Node phaseOne = new Sequence(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance),
                new TaskAttack(_transform, playerTransform, intervalBetweenShots, projectileCount, waveCooldown, projectile, dashSpeed, dashDuration)
            });

            Node phaseTwo = new Sequence(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance),
                new TaskAttack(_transform, playerTransform,
                    intervalBetweenShots - 0.05f, projectileCount * 2,
                    waveCooldown - 1, projectile,
                    dashSpeed * 1.5f, dashDuration - 0.1f
                    )
                // new TaskLaser
            });

            if (_currentPhase == 1)
            {
                root.SetChildren(new List<Node>
                {
                    phaseOne
                }, forceRoot: true);
            }
            else
            {
                root.SetChildren(new List<Node>
                {
                    phaseTwo
                }, forceRoot: true);
            }
            

            return root;
        }

        private void Update()
        {
            _rootNode.Evaluate();
        }

        public void ReceiveDamage()
        {
            health -= 1;
            if (health <= 3)
            {
                _currentPhase = 2;
                _rootNode = SetupTree();
            }
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Boss hit player");
                GameEvents.PlayerDeath?.Invoke();
            }
        }
    }
}