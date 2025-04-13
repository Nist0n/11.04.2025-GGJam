using GameControl;
using Settings.Audio;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private Button playButton;
        [SerializeField] private Button settingsButton;
        [SerializeField] private Button exitButton;
        [SerializeField] private Button backButton;

        [SerializeField] private GameObject controlButtons;
        [SerializeField] private GameObject settingsUI;

        private FaderExample _fader;
        
        
        private void Awake()
        {
            playButton.onClick.AddListener(PlayGame);
            settingsButton.onClick.AddListener(ToggleSettings);
            exitButton.onClick.AddListener(ExitGame);
            backButton.onClick.AddListener(ToggleSettings);
        }

        private void Start()
        {
            _fader = FindFirstObjectByType<FaderExample>();
            AudioManager.instance.PlayMusic("MainMenu");
            AudioManager.instance.PlayAmbient("AmbientMenu");
            settingsUI.SetActive(false);
        }

        private void PlayGame()
        {
            AudioManager.instance.PlaySfx("Click");
            _fader.LoadScene("Room");
            AudioManager.instance.PlayAmbient("Ambient");
        }

        private void ToggleSettings()
        {
            AudioManager.instance.PlaySfx("Click");
            controlButtons.SetActive(!controlButtons.activeSelf);
            settingsUI.SetActive(!settingsUI.activeSelf);
            
            if (settingsUI.activeSelf)
            {
                EventSystem.current.SetSelectedGameObject(backButton.gameObject);    
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(playButton.gameObject);
            }
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
