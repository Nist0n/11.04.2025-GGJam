using System;
using Settings.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Settings
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button backButton;

        [SerializeField] private GameObject controlButtons;
        [SerializeField] private GameObject settingsUI;
        
        private void Awake()
        {
            playButton.onClick.AddListener(PlayGame);
            settingsButton.onClick.AddListener(ToggleSettings);
            exitButton.onClick.AddListener(ExitGame);
            backButton.onClick.AddListener(ToggleSettings);
        }

        private void Start()
        {
            AudioManager.instance.PlayMusic("MainMenu");
            settingsUI.SetActive(false);
        }

        private void PlayGame()
        {
            AudioManager.instance.PlaySfx("Click");
            // SceneManager.LoadScene(1);
            AudioManager.instance.StartMusicShuffle();
        }

        private void ToggleSettings()
        {
            AudioManager.instance.PlaySfx("Click");
            controlButtons.SetActive(!controlButtons.activeSelf);
            settingsUI.SetActive(!settingsUI.activeSelf);
        }

        private void ExitGame()
        {
            AudioManager.instance.PlaySfx("Click");
            #if UNITY_EDITOR
                EditorApplication.isPlaying = false;
            #else
                Application.Quit();
            #endif
        }
    }
}
