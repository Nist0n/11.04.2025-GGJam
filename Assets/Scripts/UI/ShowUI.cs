using Static_Classes;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class ShowUI : MonoBehaviour
    {
        [SerializeField] private GameObject loseUI;
        
        [SerializeField] private Button tryAgainOnSecondPhaseButton;
        
        private void ShowLoseUI()
        {
            loseUI.SetActive(true);
            if (PlayerPrefs.GetInt("SecondPhaseAchieved") == 1)
            {
                tryAgainOnSecondPhaseButton.gameObject.SetActive(true);
            }
            else
            {
                tryAgainOnSecondPhaseButton.gameObject.SetActive(false);
            }
        }
        
        private void OnDisable()
        {
            EventHandler.PlayerDeath -= ShowLoseUI;
        }

        private void OnEnable()
        {
            EventHandler.PlayerDeath += ShowLoseUI;
        }
    }
}
