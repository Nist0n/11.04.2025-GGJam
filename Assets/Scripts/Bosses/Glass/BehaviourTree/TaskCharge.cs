using AI.BehaviourTree.Base;
using UnityEngine;

namespace Bosses.Glass.BehaviourTree
{
    public class TaskCharge : Node
    {
        private Transform _transform;
        private Transform _playerTransform;
        private float _chargeSpeed;

        public TaskCharge(Transform transform, Transform playerTransform, float chargeSpeed)
        {
            _transform = transform;
            _playerTransform = playerTransform;
            _chargeSpeed = chargeSpeed;
        }

        public override NodeState Evaluate()
        {
            _state = NodeState.Running;

            return _state;
        }
    }
}