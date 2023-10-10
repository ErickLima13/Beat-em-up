using UnityEngine;

namespace PainfulSmile.Runtime.Core.Scriptables
{
    [CreateAssetMenu(fileName = "New Delay", menuName = PainfulSmileKeys.ScriptablePath + "Delay")]
    public class DelayData : ScriptableObject
    {
        public float delay;
    }
}
