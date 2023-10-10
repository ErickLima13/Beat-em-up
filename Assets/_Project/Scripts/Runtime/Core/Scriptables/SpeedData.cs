using UnityEngine;

namespace PainfulSmile.Runtime.Core.Scriptables
{
    [CreateAssetMenu(fileName = "New Speed", menuName = PainfulSmileKeys.ScriptablePath + "Speed")]
    public class SpeedData : ScriptableObject
    {
        public float speed;
    }

}

