using System.Collections.Generic;
using System.Linq;

namespace AI.BehaviourTree.Base
{
    public class Sequence : Node
    {
        private bool _isRandom;

        public Sequence() : base() { _isRandom = false; }
        public Sequence(bool isRandom) : base() { _isRandom = isRandom; }
        public Sequence(List<Node> children, bool isRandom = false) : base(children)
        {
            _isRandom = isRandom;
        }

        public static List<T> Shuffle<T>(List<T> list)
        {
            System.Random r = new System.Random();
            return list.OrderBy(x => r.Next()).ToList();
        }

        public override NodeState Evaluate()
        {
            bool anyChildIsRunning = false;
            if (_isRandom)
                children = Shuffle(children);

            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case NodeState.Failure:
                        _state = NodeState.Failure;
                        return _state;
                    case NodeState.Success:
                        continue;
                    case NodeState.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        _state = NodeState.Success;
                        return _state;
                }
            }
            _state = anyChildIsRunning ? NodeState.Running : NodeState.Success;
            return _state;
        }
    }
}