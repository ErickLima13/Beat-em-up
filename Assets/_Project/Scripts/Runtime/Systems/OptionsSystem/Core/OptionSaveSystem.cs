using PainfulSmile.Runtime.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.Core
{
    public class OptionSaveSystem
    {
        private readonly OptionsManager _manager;
        private readonly List<OptionReference> _optionPlayerPrefsReferences = new();

        public bool IsInitialized { get; private set; }

        public OptionSaveSystem(OptionsManager optionsManager)
        {
            _manager = optionsManager;

            Initialize();
        }

        private void Initialize()
        {
            StorePlayerPrefsKeys();

            IsInitialized = true;
        }

        private void StorePlayerPrefsKeys()
        {
            if (_optionPlayerPrefsReferences.Count > 0)
            {
                return;
            }

            OptionReference mouseSensibilityReference = CreateReference(OptionType.MOUSE_SENSIBILITY, PainfulSmileKeys.Options.MouseSensibilityKey);
            OptionReference fovReference = CreateReference(OptionType.FOV, PainfulSmileKeys.Options.FOVKey);

            OptionReference gamepadVibrationReference = CreateReference(OptionType.GAMEPAD_VIBRATION_TOGGLE, PainfulSmileKeys.Options.GamepadVibrationKey);

            OptionReference vsyncReference = CreateReference(OptionType.VSYNC_TOGGLE, PainfulSmileKeys.Options.VsyncKey);
            OptionReference fullscreen = CreateReference(OptionType.FULLSCREEN_TOGGLE, PainfulSmileKeys.Options.FullscreenKey);
            OptionReference resolutionReference = CreateReference(OptionType.RESOLUTION_INDEX, PainfulSmileKeys.Options.ResolutionKey);

            _optionPlayerPrefsReferences.Add(mouseSensibilityReference);
            _optionPlayerPrefsReferences.Add(fovReference);

            _optionPlayerPrefsReferences.Add(gamepadVibrationReference);

            _optionPlayerPrefsReferences.Add(vsyncReference);
            _optionPlayerPrefsReferences.Add(fullscreen);
            _optionPlayerPrefsReferences.Add(resolutionReference);
        }

        public void SaveToPrefsFloat(OptionType optionType, float value)
        {
            OptionReference optionReference = GetReference(optionType);

            PlayerPrefs.SetFloat(optionReference.key, value);
            PlayerPrefs.Save();
        }

        public void SaveToPrefsInt(OptionType optionType, int value)
        {
            OptionReference optionReference = GetReference(optionType);

            PlayerPrefs.SetInt(optionReference.key, value);
            PlayerPrefs.Save();
        }

        public int LoadFromPrefsInt(OptionType optionType, int defaultValue)
        {
            OptionReference optionReference = GetReference(optionType);

            if (optionReference == null)
            {
                return defaultValue;
            }

            if (PlayerPrefs.HasKey(optionReference.key))
            {
                return PlayerPrefs.GetInt(optionReference.key);
            }

            return defaultValue;
        }

        public float LoadFromPrefsFloat(OptionType optionType, float defaultValue)
        {
            OptionReference optionReference = GetReference(optionType);

            if (optionReference == null)
            {
                return defaultValue;
            }

            if (PlayerPrefs.HasKey(optionReference.key))
            {
                return PlayerPrefs.GetFloat(optionReference.key);
            }

            return defaultValue;
        }

        private OptionReference CreateReference(OptionType type, string playerPrefsKey)
        {
            return new()
            {
                type = type,
                key = playerPrefsKey,
            };
        }

        private OptionReference GetReference(OptionType type)
        {
            OptionReference optionReference = null;

            foreach (OptionReference reference in _optionPlayerPrefsReferences)
            {
                if (reference.type != type)
                    continue;

                optionReference = reference;
            }

            return optionReference;
        }

        public void DeleteOptions()
        {
            foreach (OptionReference reference in _optionPlayerPrefsReferences)
            {
                PlayerPrefs.DeleteKey(reference.key);
            }
        }
    }
}