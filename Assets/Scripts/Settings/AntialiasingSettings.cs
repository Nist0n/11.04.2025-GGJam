using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace Settings
{
    public class AntialiasingSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown cameraAaDropdown;
        [SerializeField] private TMP_Dropdown msaaDropdown;

        [SerializeField] private List<String> options;

        public const string CameraAaKey = "CAMERA_AA";
        public const string MSAAKey = "MSAA";
        
        private void Start()
        {
            // Set saved values
            int aaModeIndex = PlayerPrefs.GetInt(CameraAaKey, 0);
            int sampleCount = PlayerPrefs.GetInt(MSAAKey, 1);

            cameraAaDropdown.value = aaModeIndex;
            ChangeAASettings(aaModeIndex, Camera.main);
            msaaDropdown.value = SampleCountToDropdownValue(sampleCount);
            ChangeMSAASettings(sampleCount);
        }

        public void OnCameraAaDropdownChanged()
        {
            int currentValue = cameraAaDropdown.value;
            ChangeAASettings(currentValue, Camera.main);
            AdjustMSAADropdown(currentValue);
        }

        public void OnMsaaDropdownChanged()
        {
            int currentValue = msaaDropdown.value;
            ChangeMSAASettings(DropdownValueToSampleCount(currentValue));
            
            AdjustAADropdown();
        }
        
        /// <summary>
        /// Changes antialiasing parameter in camera rendering settings.
        /// </summary>
        /// <param name="aaModeIndex">
        ///     Antialiasing mode. 0 - No antialiasing, 1 - FXAA, 2 - SMAA, <br/> 3 - TAA. <br/>
        ///     Note that TAA only works when MSAA is off. <br/>
        ///     If MSAA is on and 3 is passed, antialiasing will be set to SMAA instead.
        /// </param>
        /// /// <param name="cameraObject">
        ///     Camera object to set antialiasing for.
        /// </param>
        public static void ChangeAASettings(int aaModeIndex, Camera cameraObject)
        {
            UniversalRenderPipelineAsset renderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            if (!renderPipelineAsset)
            {
                Debug.LogWarning("No render pipeline asset found");
                return;
            }
            
            if (aaModeIndex == 3 && renderPipelineAsset.msaaSampleCount != 1) // if MSAA is on
            {
                Debug.Log("MSAA is on which conflicts with TAA, SMAA will be set instead.");
                aaModeIndex = 2;
            }
            AntialiasingMode aaMode = (AntialiasingMode) aaModeIndex;
            
            if (aaMode == cameraObject.GetComponent<UniversalAdditionalCameraData>().antialiasing)
            {
                return;
            }
            
            cameraObject.GetComponent<UniversalAdditionalCameraData>().antialiasing = aaMode;
            PlayerPrefs.SetInt(CameraAaKey, aaModeIndex);
        }

        /// <summary>
        /// Changes MSAA setting for current URP asset.
        /// </summary>
        /// <param name="sampleCount">
        ///     MSAA sample count. <br/> 1 - MSAA disabled. Other possible values are 2, 4, 8.
        /// </param>
        public static void ChangeMSAASettings(int sampleCount)
        {
            UniversalRenderPipelineAsset renderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            if (!renderPipelineAsset)
            {
                Debug.LogWarning("No render pipeline asset found");
                return;
            }

            if (renderPipelineAsset.msaaSampleCount == sampleCount)
            {
                return;
            }

            if (IsTaaEnabled())
            {
                
            }
            
            List<int> possibleValues = new() { 1, 2, 4, 8 };
            if (!possibleValues.Contains(sampleCount))
            {
                Debug.LogWarning("Invalid sample count \n Possible values are: 1, 2, 4, 8");
            }
            
            renderPipelineAsset.msaaSampleCount = sampleCount;
            PlayerPrefs.SetInt(MSAAKey, sampleCount);
        }

        private void AdjustAADropdown()
        {
            // If MSAA is on, TAA needs to be turned off
            UniversalRenderPipelineAsset renderPipelineAsset = QualitySettings.renderPipeline as UniversalRenderPipelineAsset;
            if (!renderPipelineAsset)
            {
                Debug.LogWarning("No render pipeline asset found");
                return;
            }
            
            if (renderPipelineAsset.msaaSampleCount != 1)
            {
                int currentValue = cameraAaDropdown.value;
                cameraAaDropdown.ClearOptions();
                cameraAaDropdown.AddOptions(options.GetRange(0, 3));
                cameraAaDropdown.value = currentValue == 3 ? 2 : currentValue; // Possibly redundant
            }
            else
            {
                cameraAaDropdown.AddOptions(options.GetRange(3, 1));
            }
        }

        private void AdjustMSAADropdown(int aaModeIndex)
        {
            // If TAA is on, MSAA needs to be turned off
            if (aaModeIndex == 3)
            {
                msaaDropdown.value = 0;
                msaaDropdown.interactable = false;
            }
            else
            {
                msaaDropdown.interactable = true;
            }
        }

        private static bool IsTaaEnabled()
        {
            if (Camera.main == null)
            {
                Debug.LogWarning("No main camera found");
                return false;
            }
            
            return Camera.main.GetComponent<UniversalAdditionalCameraData>().antialiasing == AntialiasingMode.TemporalAntiAliasing;
        }

        private int SampleCountToDropdownValue(int sampleCount)
        {
            int dropdownValue = sampleCount switch
            {
                2 => 1,
                4 => 2,
                8 => 3,
                _ => 0
            };

            return dropdownValue;
        }
        
        private int DropdownValueToSampleCount(int dropdownValue)
        {
            int sampleCount = dropdownValue switch
            {
                1 => 2,
                2 => 4,
                3 => 8,
                _ => 1
            };

            return sampleCount;
        }
    }
}