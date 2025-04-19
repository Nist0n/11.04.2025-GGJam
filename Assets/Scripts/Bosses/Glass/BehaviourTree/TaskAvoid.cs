using AI.BehaviourTree.Base;
using UnityEngine;
using UnityEngine.AI;

namespace Bosses.Glass.BehaviourTree
{
    public class TaskAvoid : Node
    {
        private NavMeshAgent _agent;
        private Transform _playerTransform;
        private Transform _transform;
        private Transform _targetTransform;
        private float _avoidDistance;

        private LaserController _laserController;
        
        public TaskAvoid(NavMeshAgent agent, Transform playerTransform, Transform transform, Transform targetTransform, float avoidDistance, LaserController laserController)
        {
            _agent = agent;
            _playerTransform = playerTransform;
            _transform = transform;
            _targetTransform = targetTransform;
            _avoidDistance = avoidDistance;
            _laserController = laserController;
        }

        public override NodeState Evaluate()
        {
            _state = NodeState.Running;
            // avoid player (keep certain distance)
            _agent.SetDestination(_targetTransform.position);
            
            Vector3 directionToPlayer = _playerTransform.position - _transform.position;
            if (_laserController.CurrentState != LaserState.Lasering && _laserController.CurrentState != LaserState.Preparing)
            {
                _transform.rotation = Quaternion.LookRotation(directionToPlayer);
            }
            
            float sqrDistance = Vector3.SqrMagnitude(directionToPlayer);
            if (sqrDistance < Mathf.Pow(_avoidDistance, 2))
            {
                Vector3 desiredPosition = -directionToPlayer.normalized * 5;
                _agent.SetDestination(_transform.position + desiredPosition);
            }
            else
            {
                _state = NodeState.Success;
            }
            return _state;
        }
    }
}