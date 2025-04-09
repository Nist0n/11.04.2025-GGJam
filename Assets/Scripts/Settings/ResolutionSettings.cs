using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class ResolutionSettings : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullScreenToggle;
        
        private List<Resolution> _resolutions;
        
        private void Start()
        {
            _resolutions = GetResolutions();
            resolutionDropdown.ClearOptions();
            
            List<string> options = new List<string>();
            foreach (Resolution res in _resolutions)
            {
                options.Add($"{res.width}x{res.height}");
            }
            resolutionDropdown.AddOptions(options);
            
            int defaultResolutionIndex = _resolutions.FindIndex(x => x is { width: 1920, height: 1080 });
            
            int width = PlayerPrefs.GetInt("ResolutionWidth", 0);
            int height = PlayerPrefs.GetInt("ResolutionHeight", 0);
            
            if (width != 0 && height != 0)
            {
                defaultResolutionIndex = _resolutions.FindIndex(x => x.width == width && x.height == height);
            }
            
            resolutionDropdown.value = defaultResolutionIndex;
            // By default, full screen should be on
            fullScreenToggle.isOn = PlayerPrefs.GetInt("FullScreen", 1) == 1;
        }
        
        public void OnResolutionChanged()
        {
            bool fullScreen = fullScreenToggle.isOn;
            Resolution resolution = _resolutions[resolutionDropdown.value];
            
            if (Screen.width == resolution.width && Screen.height == resolution.height)
            {
                return;
            }
            
            Screen.SetResolution(resolution.width, resolution.height, fullScreen);
            PlayerPrefs.SetInt("ResolutionWidth", resolution.width);
            PlayerPrefs.SetInt("ResolutionHeight", resolution.height);
        }

        public void OnFullScreenValueChanged()
        {
            Screen.fullScreen = fullScreenToggle.isOn;
            PlayerPrefs.SetInt("FullScreen", fullScreenToggle.isOn ? 1 : 0);
        }
        
        private static List<Resolution> GetResolutions() {
            //Filters out all resolutions with low refresh rate:

            Resolution[] resolutions = Screen.resolutions;
            HashSet<Tuple<int, int>> uniqResolutions = new HashSet<Tuple<int, int>>();
            Dictionary<Tuple<int, int>, RefreshRate> maxRefreshRates = new Dictionary<Tuple<int, int>, RefreshRate>();

            for (int i = 0; i < resolutions.GetLength(0); i++) {
                //Add resolutions (if they are not already contained)
                Tuple<int, int> resolution = new Tuple<int, int>(resolutions[i].width, resolutions[i].height);
                uniqResolutions.Add(resolution);
                //Get highest framerate:
                maxRefreshRates[resolution] = resolutions[i].refreshRateRatio;
            }
            //Build resolution list:
            List<Resolution> uniqResolutionsList = new List<Resolution>(uniqResolutions.Count);
            foreach (Tuple<int, int> resolution in uniqResolutions) {
                Resolution newResolution = new Resolution();
                newResolution.width = resolution.Item1;
                newResolution.height = resolution.Item2;
                if(maxRefreshRates.TryGetValue(resolution, out RefreshRate refreshRate)) {
                    newResolution.refreshRateRatio = refreshRate;
                }
                uniqResolutionsList.Add(newResolution);
            }
            
            return uniqResolutionsList;
        }
    }
}