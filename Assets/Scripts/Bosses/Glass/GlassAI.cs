using System.Collections;
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
        [SerializeField] private float rotationSpeed;
        
        private Node _rootNode;

        private NavMeshAgent _agent;
        private Transform _transform;
        private Rigidbody _rigidbody;
        private Animator _animator;

        private int _currentPhase = 1;
        
        private void Start()
        {
            _rootNode = SetupTree();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        protected override Node SetupTree()
        {
            _agent = GetComponent<NavMeshAgent>();
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
            _animator = GetComponent<Animator>();
            
            _transform = transform;
            
            Node root = new Selector();

            LaserController laserController = new LaserController(_transform, rotationSpeed, lineRenderer, _agent);
            
            Node phaseOne = new Sequence(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance, laserController),
                new TaskAttack(_transform, playerTransform,
                    intervalBetweenShots, projectileCount,
                    waveCooldown, projectile,
                    dashSpeed, dashDuration,
                    _currentPhase, laserController, _animator
                )
            });

            Node phaseTwo = new Sequence(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance, laserController),
                new TaskAttack(_transform, playerTransform,
                    intervalBetweenShots - 0.05f, projectileCount * 2,
                    waveCooldown - 1, projectile,
                    dashSpeed * 1.5f, dashDuration - 0.1f,
                    _currentPhase, laserController, _animator
                )
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
            if (_rigidbody.constraints == RigidbodyConstraints.FreezeAll)
            {
                return;
            }
            _rootNode.Evaluate();
        }

        private void ReceiveDamage()
        {
            health -= 1;
            Debug.Log(health);
            if (health <= 3)
            {
                _currentPhase = 2;
                _rootNode = SetupTree();
            }

            if (health <= 0)
            {
                StartCoroutine(WaitForDeathAnimation());
            }
        }
        
        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                Debug.Log("Boss hit player");
                GameEvents.PlayerDeath?.Invoke();
            }

            if (other.gameObject.CompareTag("Spell"))
            {
                StartCoroutine(Stun());
            }

            if (other.gameObject.CompareTag("Trap"))
            {
                ReceiveDamage();
            }
        }

        private IEnumerator Stun()
        {
            _animator.Play("Eye Close");
            _agent.isStopped = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitForSeconds(3f);
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _agent.isStopped = false;
        }

        private IEnumerator WaitForDeathAnimation()
        {
            _animator.Play("Death");
            yield return new WaitForSeconds(2.6f);
            Destroy(gameObject);
        }
    }
}