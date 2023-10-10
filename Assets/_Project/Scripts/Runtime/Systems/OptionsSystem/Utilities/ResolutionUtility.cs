using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.Utilities
{
    public static class ResolutionUtility
    {
        public static List<Resolution> GetCompatibleResolutions()
        {
            Resolution[] resolutions = Screen.resolutions;
            List<Resolution> compatibleResolutions = new();

            double refreshRate = Screen.currentResolution.refreshRateRatio.value;

            for (int i = 0; i < resolutions.Length; i++)
            {
                if (resolutions[i].refreshRateRatio.value == refreshRate)
                {
                    compatibleResolutions.Add(resolutions[i]);
                }
            }

            return compatibleResolutions;
        }

        public static int GetCurrentResolutionIndex(List<Resolution> resolutions)
        {
            for (int i = 0; i < resolutions.Count; i++)
            {
                if (resolutions[i].width == Screen.width && resolutions[i].height == Screen.height)
                {
                    return i;
                }
            }

            return -1;
        }
    }
}