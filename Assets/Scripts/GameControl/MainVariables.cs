using System;
using UnityEngine;
using EventHandler = Static_Classes.EventHandler;

namespace GameControl
{
    public class MainVariables : MonoBehaviour
    {
        private static MainVariables _instance;

        private void SetOnSecondPhaseAchieved() => PlayerPrefs.SetInt("SecondPhaseAchieved", 1);

        private void SetOnBossCompleted()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 0);
            PlayerPrefs.SetInt("SecondPhaseAchieved", 0);
        }

        private void Awake()
        {
            PlayerPrefs.SetInt("SecondPhaseStart", 0);
            PlayerPrefs.SetInt("SecondPhaseAchieved", 0);
            
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        private void OnDisable()
        {
            EventHandler.SecondPhaseAchieved -= SetOnSecondPhaseAchieved;
            EventHandler.BossCompleted -= SetOnBossCompleted;
        }

        private void OnEnable()
        {
            EventHandler.SecondPhaseAchieved += SetOnSecondPhaseAchieved;
            EventHandler.BossCompleted += SetOnBossCompleted;
        }
    }
}
