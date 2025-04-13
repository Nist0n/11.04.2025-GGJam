

using System;
using GameControl;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace UI
{
    public class LoseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject loseUI;
        
        [SerializeField] private Button tryAgainOnSecondPhaseButton;
        
        [SerializeField] private Button tryAgainButton;

        private void Start()
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            tryAgainButton.onClick.AddListener(OnButtonClickTryAgain);
            tryAgainOnSecondPhaseButton.onClick.AddListener(OnButtonClickTryAgainOnSecondPhase);
        }

        private void OnButtonClickTryAgainOnSecondPhase()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            loseUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
        
        private void OnButtonClickTryAgain()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            loseUI.SetActive(false);
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
}
