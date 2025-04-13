using System;
using UnityEngine;

namespace GameControl
{
    public class Fade : MonoBehaviour
    {
        private const string FaderPath = "Fade";

        [SerializeField] private Animator animator;

        private static Fade _instance;
    
        public bool IsFading { get; private set; }

        private Action _fadedInCallback;
        private Action _fadedOutCallback;

        public static Fade instance
        {
            get
            {
                if (_instance == null)
                {
                    var prefab = Resources.Load<Fade>(FaderPath);
                    _instance = Instantiate(prefab);
                    DontDestroyOnLoad(_instance.gameObject);
                }

                return _instance;
            }
        }

        public void FadeIn(Action fadedInCallback)
        {
            if (IsFading)
            {
                return;
            }

            IsFading = true;
            _fadedInCallback = fadedInCallback;
            animator.SetBool("faded", true);
        }
    
        public void FadeOut(Action fadedOutCallback)
        {
            if (IsFading)
            {
                return;
            }

            IsFading = true;
            _fadedOutCallback = fadedOutCallback;
            animator.SetBool("faded", false);
        }

        private void Handle_FadeInAnimationOver()
        {
            _fadedInCallback?.Invoke();
            _fadedInCallback = null;
            IsFading = false;
        }
    
        private void Handle_FadeOutAnimationOver()
        {
            _fadedOutCallback?.Invoke();
            _fadedOutCallback = null;
            IsFading = false;
            Destroy(gameObject);
        }
    }
}