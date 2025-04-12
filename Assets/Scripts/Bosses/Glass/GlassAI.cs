using System;
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
                new Sequence(new List<Node>
                {
                    new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance),
                    new TaskShoot(_transform, playerTransform, 0.15f, 6, 2f, projectile)
                }),
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
                GameEvents.PlayerDeath?.Invoke();
            }
        }
    }
}