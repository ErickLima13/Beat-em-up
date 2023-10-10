using Cinemachine;
using UnityEngine;
using PainfulSmile.Runtime.Systems.OptionsSystem.Interfaces;
using PainfulSmile.Runtime.Systems.OptionsSystem.Camera;
using PainfulSmile.Runtime.Systems.OptionsSystem.Core;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.Cinemachine
{
    public class CinemachineFovController : MonoBehaviour, IReceiveOptionValues<float>
    {
        [Header("References")]
        [SerializeField] private CinemachineVirtualCamera _vCam;

        [Header("Camera Settings")]
        [SerializeField] private CameraSettingsData _camSettings;

        public OptionType OptionType { get; set; } = OptionType.FOV;

        private void Start()
        {
            ReceiveValue(OptionType, OptionsManager.Instance.GetValueFloat(OptionType, _camSettings.FovDefaultValue));
            RegisterGlobalCallback(this);
        }

        private void OnDestroy()
        {
            UnRegisterGlobalCallback(this);
        }

        public void RegisterGlobalCallback(IReceiveOptionValues<float> callback)
        {
            OptionsManager.Instance.OnUpdateOptionValue += ReceiveValue;
        }

        public void UnRegisterGlobalCallback(IReceiveOptionValues<float> callback)
        {
            OptionsManager.Instance.OnUpdateOptionValue -= ReceiveValue;
        }

        public void ReceiveValue(OptionType optionType, float value)
        {
            if (optionType != OptionType)
            {
                return;
            }

            _vCam.m_Lens.FieldOfView = value;
        }
    }
}