using PainfulSmile.Runtime.Systems.AudioSystem.Core;
using PainfulSmile.Runtime.Systems.AudioSystem.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers
{
    public abstract class AudioTriggerBase : MonoBehaviour
    {
        [SerializeField] private List<SoundData> _sounds;

        [Header("Settings")]
        [SerializeField] private float _delay;
        [SerializeField] private float _cooldown;

        private bool _canTrigger = true;

        public virtual void PlayFirstSound()
        {
            PlaySound(0);
        }

        [ContextMenu("Play Audio")]
        public virtual void PlaySound(int index)
        {
            if (!AudioManager.Instance)
            {
                return;
            }

            if(!_sounds[index])
            {
                return;
            }

            if (!_canTrigger)
            {
                return;
            }
            
            AudioManager.Instance.Play3DAudio(_sounds[index], transform.position, _delay);

            if (_sounds[index].Loop)
            {
                return;
            }

            if (!gameObject.activeInHierarchy)
            {
                return;
            }

            StartCoroutine(DelayAndCooldown());
        }

        public virtual void StopSound()
        {
            if (!AudioManager.Instance)
            {
                return;
            }

            AudioManager.Instance.StopAudio(_sounds);
        }
        private IEnumerator DelayAndCooldown()
        {
            _canTrigger = false;

            yield return new WaitForSeconds(_delay);
            yield return new WaitForSeconds(_cooldown);

            _canTrigger = true;
        }
    }
}