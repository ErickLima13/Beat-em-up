using PainfulSmile.Runtime.Core;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem
{
    [CreateAssetMenu(fileName = "NewFloatValue", menuName = PainfulSmileKeys.ScriptablePath + "Float Value")]
    public class FloatDefaultValueData : ScriptableObject
    {
        [field: SerializeField] public float Value { get; private set; }
    }
}