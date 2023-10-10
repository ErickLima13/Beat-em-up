using PainfulSmile.Runtime.Systems.AudioSystem.Core;
using PainfulSmile.Runtime.Systems.AudioSystem.Scriptables;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem
{
    public class ContinuouslyPlayAudio : MonoBehaviour
    {
        [SerializeField] private List<SoundData> _soundDatas = new();
        [SerializeField] private float _timeBetweenToPlay = 2f;

        private void Start()
        {
            StartCoroutine(WaitToPlaySoundS());
        }

        private IEnumerator WaitToPlaySoundS()
        {
            yield return new WaitForSeconds(_timeBetweenToPlay);

            int randIndex = Random.Range(0, _soundDatas.Count);
            AudioManager.Instance.Play3DAudio(_soundDatas[randIndex], transform.position);

            StartCoroutine(WaitToPlaySoundS());
        }
    }
}