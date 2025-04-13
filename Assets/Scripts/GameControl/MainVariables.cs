using System;
using Static_Classes;
using UnityEngine;

namespace GameControl
{
    public class MainVariables : MonoBehaviour
    {
        private static MainVariables _instance;
        
        [SerializeField] private FaderExample faderExample;

        private void SetOnSecondPhaseAchieved() => PlayerPrefs.SetInt("SecondPhaseAchieved", 1);

        private void SetOnBossCompleted()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 0);
            PlayerPrefs.SetInt("SecondPhaseAchieved", 0);
        }

        private void Awake()
        {
            if (!PlayerPrefs.HasKey("SecondPhaseStart"))
            {
                PlayerPrefs.SetInt("SecondPhaseStart", 0);
            }
            PlayerPrefs.SetInt("SecondPhaseAchieved", 0);
            
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            GameEvents.SecondPhaseAchieved -= SetOnSecondPhaseAchieved;
            GameEvents.BossCompleted -= SetOnBossCompleted;
        }

        private void OnEnable()
        {
            GameEvents.SecondPhaseAchieved += SetOnSecondPhaseAchieved;
            GameEvents.BossCompleted += SetOnBossCompleted;
        }
    }
}
