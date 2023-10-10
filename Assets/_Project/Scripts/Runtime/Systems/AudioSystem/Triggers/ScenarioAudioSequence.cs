using PainfulSmile.Runtime.Systems.AudioSystem.Core;
using PainfulSmile.Runtime.Systems.AudioSystem.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Triggers
{
    public class ScenarioAudioSequence : MonoBehaviour
    {
        [SerializeField] private List<SoundData> _soundDataSequence;
        [SerializeField] private bool _autoPlay = true;


        private void Start()
        {
            if (_autoPlay)
            {
                StartCoroutine(WaitFrame());
            }
        }

        public void PlaySequence()
        {
            if (!AudioManager.Instance)
            {
                return;
            }

            StartCoroutine(AudioSequence());
        }

        public IEnumerator AudioSequence()
        {

            for (int i = 0; i < _soundDataSequence.Count; i++)
            {
                AudioManager.Instance.Play3DAudio(_soundDataSequence[i], transform.position);
                yield return new WaitForSeconds(_soundDataSequence[i].Clip.length);
            }
        }

        private IEnumerator WaitFrame()
        {
            yield return new WaitForEndOfFrame();

            if (_autoPlay)
            {
                PlaySequence();
            }          
        }
    }
}