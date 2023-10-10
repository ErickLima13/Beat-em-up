using PainfulSmile.Runtime.Core;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem.Camera
{
    [CreateAssetMenu(fileName = "NewCameraSettings", menuName = PainfulSmileKeys.ScriptablePath + "Camera Settings")]
    public class CameraSettingsData : ScriptableObject
    {
        [Header("Mouse Sensibility Values")]
        public float MouseSensDefaultValue;
        public float MinMouseSens;
        public float MaxMouseSens;

        [Header("FOV Values")]
        public float FovDefaultValue;
        public float MinFovValue;
        public float MaxFovValue;
    }
}

