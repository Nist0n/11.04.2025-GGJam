using System;
using TMPro;
using UnityEngine;

namespace Settings
{
    public class VSyncSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown vSyncDropdown;
        
        private const string VSyncKey = "VSync";
        
        private void Start()
        {
            int vSync = PlayerPrefs.GetInt(VSyncKey, 0);
            vSyncDropdown.value = vSync;
            QualitySettings.vSyncCount = vSync;
        }

        public void OnVSyncDropdownChanged()
        {
            QualitySettings.vSyncCount = vSyncDropdown.value;
            PlayerPrefs.SetInt(VSyncKey, QualitySettings.vSyncCount);
        }
    }
}