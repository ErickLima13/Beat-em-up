using PainfulSmile.Runtime.Systems.AudioSystem.Core;
using PainfulSmile.Runtime.Systems.AudioSystem.Scriptables;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers
{
    public class PlayRandomAudio3D : MonoBehaviour
    {
        [SerializeField] private SoundData[] _sounds;

        public void Play()
        {
            if (_sounds.Length > 0)
            {
                int randSound = Random.Range(0, _sounds.Length);
                AudioManager.Instance.Play3DAudio(_sounds[randSound], transform.position);
            }
        }
    }
}