

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
            tryAgainButton.onClick.AddListener(OnButtonClickTryAgain);
            tryAgainOnSecondPhaseButton.onClick.AddListener(OnButtonClickTryAgainOnSecondPhase);
        }

        private void OnButtonClickTryAgainOnSecondPhase()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 1);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            loseUI.SetActive(false);
        }
        
        private void OnButtonClickTryAgain()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 0);
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            loseUI.SetActive(false);
        }
    }
}
