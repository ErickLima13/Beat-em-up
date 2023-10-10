using PainfulSmile.Runtime.Systems.AudioSystem.Triggers.Interfaces;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers
{
    public class ColliderAudioTrigger : AudioTriggerBase
    {
        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IAudioTriggerAgent audioAgent))
            {
                PlayFirstSound();
            }
        }
    }
}