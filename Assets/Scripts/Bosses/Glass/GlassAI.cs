using System.Collections.Generic;
using AI.BehaviourTree.Base;
using Bosses.Glass.BehaviourTree;
using UnityEngine;
using UnityEngine.AI;

namespace Bosses.Glass
{
    public class GlassAI : AI.BehaviourTree.Base.BehaviourTree
    {
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Transform targetTransform;
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
            
            Node root = new Selector();
            
            root.SetChildren(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance)
            }, forceRoot: true);

            return root;
        }

        private void Update()
        {
            _rootNode.Evaluate();
        }
    }
}