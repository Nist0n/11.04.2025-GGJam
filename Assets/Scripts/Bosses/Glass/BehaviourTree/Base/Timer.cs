using System.Collections.Generic;
using UnityEngine;

namespace AI.BehaviourTree.Base
{
    public class Timer : Node
    {
        private static readonly int Building = Animator.StringToHash("Building");
        private float _delay;
        private float _time;

        private Animator _animator;
        private GameObject _hammer;

        private AudioSource _audioSource;

        public delegate void TickEnded();
        public event TickEnded onTickEnded;

        public Timer(float delay, TickEnded onTickEnded = null) : base()
        {
            _delay = delay;
            _time = _delay;
            this.onTickEnded = onTickEnded;
        }
        public Timer(float delay, List<Node> children, TickEnded onTickEnded = null)
            : base(children)
        {
            _delay = delay;
            _time = _delay;
            this.onTickEnded = onTickEnded;
        }
        
        public Timer(float delay, List<Node> children, Animator animator, GameObject hammer, AudioSource audioSource, TickEnded onTickEnded = null)
            : base(children)
        {
            _delay = delay;
            _time = _delay;
            _animator = animator;
            _hammer = hammer;
            _audioSource = audioSource;
            _audioSource.clip = Resources.Load("Audio/SFX/construction") as AudioClip;
            _audioSource.loop = true;
            this.onTickEnded = onTickEnded;
        }
        
        public override NodeState Evaluate()
        {
            if (!HasChildren) return NodeState.Failure;
            if (!_audioSource.isPlaying)
            {
                _audioSource.Play();
            }
            _animator.SetBool(Building, true);
            _hammer.SetActive(true);
            if (_time <= 0)
            {
                _time = _delay;
                _state = children[0].Evaluate();
                onTickEnded?.Invoke();
                _animator.SetBool(Building, false);
                _hammer.SetActive(false);
                _audioSource.Stop();
                _state = NodeState.Success;
            }
            else
            {
                _time -= Time.deltaTime;
                _state = NodeState.Running;
            }
            
            return _state;
        }
    }
}