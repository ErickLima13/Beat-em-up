using PainfulSmile.Runtime.Systems.OptionsSystem.Core;
using PainfulSmile.Runtime.Systems.OptionsSystem.UI.Base;
using PainfulSmile.Runtime.Systems.OptionsSystem.Utilities;
using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.UI
{
    public class ResolutionDropDown : DropDownBase
    {
        private List<Resolution> _filteredResolutions = new();
        private readonly OptionType _resolutionOption = OptionType.RESOLUTION_INDEX;

        private void OnEnable()
        {
            SetResolutionSettings();
        }

        private void SetResolutionSettings()
        {
            _filteredResolutions = ResolutionUtility.GetCompatibleResolutions();

            _dropDown.ClearOptions();

            List<string> options = new();

            for (int i = 0; i < _filteredResolutions.Count; i++)
            {
                string resolutionOption = _filteredResolutions[i].width + "x" + _filteredResolutions[i].height + " " + _filteredResolutions[i].refreshRateRatio.value + "Hz";
                options.Add(resolutionOption); 
            }

            int resolutionIndex = OptionsManager.Instance.GetValueInt(_resolutionOption, -1);

            if (resolutionIndex == -1)
            {
                resolutionIndex = 0;
            }

            _dropDown.AddOptions(options);
            _dropDown.value = resolutionIndex;

            _dropDown.RefreshShownValue();
        }

        protected override void DropDownAction(int value)
        {
            ChangeResolution(value);
        }

        private void ChangeResolution(int index)
        {
            OptionsManager.Instance.UpdateValueInt(_resolutionOption, index);
        }
    }
}