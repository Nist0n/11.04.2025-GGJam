using System.Collections;
using System.Collections.Generic;
using AI.BehaviourTree.Base;
using Bosses.Glass.BehaviourTree;
using Settings.Audio;
using Static_Classes;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;

namespace Bosses.Glass
{
    public class GlassAI : AI.BehaviourTree.Base.BehaviourTree
    {
        [SerializeField] private int maxHealth;
        
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
        [SerializeField] private Animator animator;
        [SerializeField] private AudioSource laserSound;
        
        private Node _rootNode;

        private NavMeshAgent _agent;
        private Transform _transform;
        private Rigidbody _rigidbody;

        private int _currentPhase = 1;
        
        private int _health;
        
        private void Start()
        {
            StartCoroutine(Stun());
            StartCoroutine(PlayBattleMusic());
            _rootNode = SetupTree();
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            if (PlayerPrefs.GetInt("SecondPhaseStart") == 1)
            {
                _health = maxHealth / 2;
                StartSecondPhase();
            }
            else
            {
                _health = maxHealth;
            }
        }

        protected override Node SetupTree()
        {
            _agent = GetComponent<NavMeshAgent>();
            LineRenderer lineRenderer = GetComponent<LineRenderer>();
            _rigidbody = GetComponent<Rigidbody>();
            
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
                    _currentPhase, laserController, animator, laserSound
                )
            });

            Node phaseTwo = new Sequence(new List<Node>
            {
                new TaskAvoid(_agent, playerTransform, _transform, targetTransform, avoidDistance, laserController),
                new TaskAttack(_transform, playerTransform,
                    intervalBetweenShots - 0.05f, projectileCount * 2,
                    waveCooldown - 1, projectile,
                    dashSpeed * 1.5f, dashDuration - 0.1f,
                    _currentPhase, laserController, animator, laserSound
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
            if (_rigidbody.constraints == RigidbodyConstraints.FreezeAll || _health == 0)
            {
                return;
            }
            _rootNode.Evaluate();
        }

        private void ReceiveDamage()
        {
            _health -= 1;
            GameEvents.BossDamaged?.Invoke();
            Debug.Log(_health);
            AudioManager.instance.PlaySfx("HitEye");
            if (_health <= maxHealth / 2)
            {
                StartSecondPhase();
            }

            if (_health <= 0)
            {
                StartCoroutine(WaitForDeathAnimation());
            }
        }

        private void StartSecondPhase()
        {
            _currentPhase = 2;
            _rootNode = SetupTree();
            GameEvents.SecondPhaseAchieved?.Invoke();
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
                Destroy(other.gameObject);
            }
        }

        private IEnumerator Stun()
        {
            animator.Play("Eye Close");
            _agent.isStopped = true;
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitForSeconds(2.5f);
            _rigidbody.constraints = RigidbodyConstraints.FreezeRotation;
            _agent.isStopped = false;
        }

        private IEnumerator WaitForDeathAnimation()
        {
            animator.Play("Death");
            AudioManager.instance.PlaySfx("BossDied");
            _rigidbody.constraints = RigidbodyConstraints.FreezeAll;
            yield return new WaitForSeconds(2.6f);
            GameEvents.BossCompleted?.Invoke();
            Destroy(gameObject);
        }
        
        private IEnumerator PlayBattleMusic()
        {
            AudioManager.instance.PlayMusic("EyeOpening");
            float clipLength = AudioManager.instance.GetMusicClipLength("EyeOpening");
            yield return new WaitForSeconds(clipLength);
            AudioManager.instance.PlayMusic("EyeLoop");
        }
    }
}