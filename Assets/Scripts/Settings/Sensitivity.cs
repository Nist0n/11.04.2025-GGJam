using System;
using System.Globalization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class Sensitivity : MonoBehaviour
    {
        [SerializeField] private Slider sensitivitySlider;
        [SerializeField] private TMP_Text sensValueText;

        private void Start()
        {
            if (PlayerPrefs.HasKey("Sensitivity"))
            {
                float sensitivity = PlayerPrefs.GetFloat("Sensitivity");
                sensitivitySlider.value = sensitivity;
                sensValueText.text = sensitivity.ToString(CultureInfo.CurrentCulture);
            }
            
            sensitivitySlider.onValueChanged.AddListener(delegate { ChangeSensitivity(); });
        }

        private void ChangeSensitivity()
        {
            float sensitivity = sensitivitySlider.value;
            sensValueText.text = sensitivity.ToString(CultureInfo.CurrentCulture);
            PlayerPrefs.SetFloat("Sensitivity", sensitivity);
        }
    }
}