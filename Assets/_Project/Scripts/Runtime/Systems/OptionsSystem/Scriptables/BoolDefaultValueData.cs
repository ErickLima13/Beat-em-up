using PainfulSmile.Runtime.Core;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.OptionsSystem
{
    [CreateAssetMenu(fileName = "NewBoolValue", menuName = PainfulSmileKeys.ScriptablePath + "Bool Value")]
    public class BoolDefaultValueData : ScriptableObject
    {
       [field: SerializeField] public bool Value { get; private set; }
    }
}