using PainfulSmile.Runtime.Core;
using PainfulSmile.Runtime.Systems.OptionsSystem.Camera;
using PainfulSmile.Runtime.Systems.OptionsSystem.Utilities;
using System;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.Core
{
    public class OptionsManager : Singleton<OptionsManager>
    {
        public event Action<OptionType, float> OnUpdateOptionValue;

        private OptionSaveSystem _saveSystem;

        [SerializeField] private CameraSettingsData _cameraDefaultData;
        [SerializeField] private BoolDefaultValueData _vsyncDefaultData;
        [SerializeField] private BoolDefaultValueData _gamepadVibrationDefaultData;
        [SerializeField] private BoolDefaultValueData _fullscreenDefaultData;

        protected override void Awake()
        {
            base.Awake();

            _saveSystem = new OptionSaveSystem(this);
        }

        private void Start()
        {
            OnUpdateOptionValue += UpdateOptions;

            InitializeGlobalSettings();
        }

        private void OnDestroy()
        {
            OnUpdateOptionValue -= UpdateOptions;
        }

        private void InitializeGlobalSettings()
        {
            float sensibility = GetValueFloat(OptionType.MOUSE_SENSIBILITY, _cameraDefaultData.MouseSensDefaultValue);
            float fov = GetValueFloat(OptionType.FOV, _cameraDefaultData.FovDefaultValue);

            int vibration = GetValueInt(OptionType.GAMEPAD_VIBRATION_TOGGLE, _gamepadVibrationDefaultData.Value ? 1 : 0);

            int resolution = GetValueInt(OptionType.RESOLUTION_INDEX, ResolutionUtility.GetCurrentResolutionIndex(ResolutionUtility.GetCompatibleResolutions()));
            int vsync = GetValueInt(OptionType.VSYNC_TOGGLE, _vsyncDefaultData.Value ? 1 : 0);
            int fullscreen = GetValueInt(OptionType.FULLSCREEN_TOGGLE, _fullscreenDefaultData.Value ? 1 : 0);

            UpdateOptions(OptionType.MOUSE_SENSIBILITY, sensibility);
            UpdateOptions(OptionType.FOV, fov);

            UpdateOptions(OptionType.GAMEPAD_VIBRATION_TOGGLE, vibration);

            UpdateOptions(OptionType.RESOLUTION_INDEX, resolution);
            UpdateOptions(OptionType.VSYNC_TOGGLE, vsync);
            UpdateOptions(OptionType.FULLSCREEN_TOGGLE, fullscreen);
        }

        public float GetValueFloat(OptionType optionType, float defaultValue)
        {
            return _saveSystem.LoadFromPrefsFloat(optionType, defaultValue);
        }

        public int GetValueInt(OptionType optionType, int defaultValue)
        {
            return _saveSystem.LoadFromPrefsInt(optionType, defaultValue);
        }

        public void UpdateValueFloat(OptionType optionType, float value)
        {
            _saveSystem.SaveToPrefsFloat(optionType, value);

            OnUpdateOptionValue?.Invoke(optionType, value);
        }

        public void UpdateValueInt(OptionType optionType, int value)
        {
            _saveSystem.SaveToPrefsInt(optionType, value);

            OnUpdateOptionValue?.Invoke(optionType, value);
        }

        public void DeletePrefs()
        {
            _saveSystem.DeleteOptions();
        }

        private void UpdateOptions(OptionType option, float value)
        {
            switch (option)
            {
                case OptionType.RESOLUTION_INDEX:

                    if (value < 0)
                    {
                        value = 0;
                    }

                    Resolution resolution = ResolutionUtility.GetCompatibleResolutions()[Mathf.RoundToInt(value)];
                    Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);

                    break;

                case OptionType.VSYNC_TOGGLE:

                    QualitySettings.vSyncCount = Mathf.RoundToInt(value);

                    break;

                case OptionType.FULLSCREEN_TOGGLE:

                    Screen.fullScreen = value == 1;

                    break;
            }
        }
    }
}