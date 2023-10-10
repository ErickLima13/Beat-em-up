using PainfulSmile.Runtime.Systems.AudioSystem.Core;
using PainfulSmile.Runtime.Systems.AudioSystem.Scriptables;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Utilities
{
    public class AutoMusicPlay : MonoBehaviour
    {
        [SerializeField] private SoundData _music;

        private void Start()
        {
            AudioManager.Instance.ChangeMainMusic(_music);
        }
    }
}