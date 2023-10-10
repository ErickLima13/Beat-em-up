using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Core
{
    public class AudioTracker : MonoBehaviour
    {
        private AudioSource _source;
        private Transform _target;

        private float _currentTimer;
        private float _maxTimer;
        private bool _autoStopAudioOnDestroyTrackedObject;
        private bool _initialized;

        public void SetTarget(AudioSource source, Transform target, float timer = 0f, bool autoStopAudioOnDestroyTrackedObject = false)
        {
            _source = source;
            _target = target;
            _maxTimer = timer;
            _autoStopAudioOnDestroyTrackedObject = autoStopAudioOnDestroyTrackedObject;

            _initialized = true;
        }

        private void LateUpdate()
        {
            if (!_initialized)
            {
                return;
            }

            if (!_target)
            {
                if (_autoStopAudioOnDestroyTrackedObject)
                {
                    _source.Stop();
                    _initialized = false;
                    _target = null;
                }

                return;
            }

            _currentTimer += Time.deltaTime;

            if (_currentTimer >= _maxTimer)
            {
                _currentTimer = 0;

                transform.position = _target.position;
            }
        }
    }
}