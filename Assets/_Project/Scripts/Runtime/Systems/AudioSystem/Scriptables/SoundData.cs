using UnityEngine;
using UnityEngine.Audio;
using PainfulSmile.Runtime.Core;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Scriptables
{
    [CreateAssetMenu(fileName = "New Sound Data", menuName = PainfulSmileKeys.ScriptablePath + "Sound")]
    public class SoundData : ScriptableObject
    {
        public AudioClip Clip;

        [Range(0f, 1f)]
        public float Volume = 1f;

        [Range(.1f, 3f)]
        public float Pitch = 1f;

        [Range(0f, 1f)]
        public float PitchVariation = 0;

        public float minDistance = 3f;
        public float maxDistance = 500f;
        public bool Loop = false;
        public bool is3D = true;

        public AudioMixerGroup mixerGroup;
    }
}