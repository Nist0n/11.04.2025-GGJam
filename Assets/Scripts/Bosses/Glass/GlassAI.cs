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
        
        private Node _rootNode;

        private NavMeshAgent _agent;
        private Transform _transform;

        private void Start()
        {
            _rootNode = SetupTree();
        }

        protected override Node SetupTree()
        {
            _agent = GetComponent<NavMeshAgent>();
            _transform = transform;
            LayerMask wallMask = LayerMask.GetMask("Wall");
            BossDashController bossDashController = new BossDashController(_transform, wallMask);
            
            Node root = new Selector();

            Node phaseOne = new Sequence(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance),
                new TaskAttack(_transform, playerTransform, intervalBetweenShots, projectileCount, waveCooldown, projectile, bossDashController)
            });
            
            root.SetChildren(new List<Node>
            {
                phaseOne
            }, forceRoot: true);

            return root;
        }

        private void Update()
        {
            _rootNode.Evaluate();
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