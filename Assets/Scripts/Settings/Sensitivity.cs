using System;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class Sensitivity : MonoBehaviour
    {
        [SerializeField] private Slider sensitivitySlider;

        private void Start()
        {
            if (PlayerPrefs.HasKey("Sensitivity"))
            {
                float sensitivity = PlayerPrefs.GetFloat("Sensitivity");
                sensitivitySlider.value = sensitivity;
            }
            
            sensitivitySlider.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
        }

        private void ChangeSensitivity()
        {
            float sensitivity = sensitivitySlider.value;
            PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        }
    }
}