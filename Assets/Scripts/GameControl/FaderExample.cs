using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace GameControl
{
    public class FaderExample : MonoBehaviour
    {
        private bool _isLoading;

        private static FaderExample _instance;

        private void Awake()
        {
            if (_instance != null)
            {
                Destroy(gameObject);
                return;
            }

            _instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void LoadScene(string sceneName)
        {
            if (_isLoading)
            {
                return;
            }
        
            var currentSceneName = SceneManager.GetActiveScene().name;
            if (currentSceneName == sceneName)
            {
                throw new Exception("ffffffff");
            }

            StartCoroutine(LoadSceneRoutine(sceneName));
        }

        private IEnumerator LoadSceneRoutine(string sceneName)
        {
            _isLoading = true;

            var waitFading = true;
            Fade.instance.FadeIn(() => waitFading = false);

            while (waitFading) yield return null;

            var async = SceneManager.LoadSceneAsync(sceneName);
            async.allowSceneActivation = false;

            while (async.progress < 0.9f)
            {
                yield return null;
            }

            async.allowSceneActivation = true;

            waitFading = true;
            Fade.instance.FadeOut(() => waitFading = false);

            while (waitFading)
            {
                yield return null;
            }

            _isLoading = false;
        }
    }
}