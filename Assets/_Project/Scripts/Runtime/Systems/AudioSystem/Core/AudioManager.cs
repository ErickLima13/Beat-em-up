using PainfulSmile.Runtime.Core;
using PainfulSmile.Runtime.Systems.AudioSystem.Scriptables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using Random = UnityEngine.Random;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Core
{
    public class AudioManager : Singleton<AudioManager>
    {
        [Header("Mixer Reference and Values")]
        [SerializeField] private AudioMixer _mixer;

        [Header("Audio Settings")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _ambienceSource;

        [Header("Pool Settings")]
        [SerializeField] private int _poolListSize;

        [field: Header("Default Mixer Audio Settings")]
        [field: SerializeField] public float DefaultValue { get; private set; } = -30f;
        [field: SerializeField] public float MinValue { get; private set; } = -80f;
        [field: SerializeField] public float MaxValue { get; private set; } = 0f;
        [field: SerializeField] public bool DefaultMutedValue { get; private set; } = false;

        private readonly List<AudioSource> _sourceList = new();
        private const string AudioSourceNamePrefix = "TempAudio_";

        private AudioSaveSystem _saveSystem;
        private SoundData _currentMusicData;

        protected override void Awake()
        {
            base.Awake();

            _saveSystem = new AudioSaveSystem(this);
        }

        private void Start()
        {
            _saveSystem.SetDefaultMixerVolumes();
            _saveSystem.SetDefaultMixerMutes();

            InitializePool();
        }

        private void InitializePool()
        {
            for (int i = 0; i < _poolListSize; i++)
            {
                InstantiateAudioSource(AudioSourceNamePrefix + i.ToString());
            }
        }

        public void ChangeMainMusic(SoundData data)
        {
            if (data == _currentMusicData)
            {
                return;
            }

            _currentMusicData = data;

            SetSourceSettings(data, _musicSource, 0f);
            _musicSource.Play();
        }

        /// <summary>
        /// Dont use yet, its comming in version 1.1.0
        /// </summary>
        public void ChangeMainAmbience(SoundData data)
        {
            SetSourceSettings(data, _ambienceSource);
            _ambienceSource.Play();
        }
        public float GetCurrentAudioValue(AudioSourceType audioType)
        {
            return _saveSystem.LoadVolumeValue(audioType);
        }

        public bool GetMuteAudioValue(AudioSourceType audioType)
        {
            return _saveSystem.LoadMuteValue(audioType);
        }

        public void SetMixerVolume(AudioSourceType audioType, float volume)
        {
            _mixer.SetFloat(audioType.ToString(), volume);

            _saveSystem.SaveAudioValue(audioType, volume);
        }

        public void SetMute(AudioSourceType audioType, bool muteValue, float newVolumeValue)
        {
            SetMixerVolume(audioType, newVolumeValue);

            _saveSystem.SaveMuteValue(audioType, muteValue);
        }

        /// <summary>
        /// Dont use yet, its comming in version 1.1.0
        /// </summary>
        public void Play3DAudio(SoundData sound, Transform trackedTransform, float delay = 0)
        {
            AudioSource currentSource = GetOrCreateSource();
            AudioTracker tracker = currentSource.GetComponent<AudioTracker>();

            tracker.SetTarget(currentSource, trackedTransform);

            SetSourceSettings(sound, currentSource);

            currentSource.PlayDelayed(delay);
        }

        public void Play3DAudio(SoundData sound, Vector3 soundPosition, float delay = 0)
        {
            AudioSource currentSource = GetOrCreateSource();

            currentSource.transform.position = soundPosition;

            SetSourceSettings(sound, currentSource);

            currentSource.PlayDelayed(delay);
        }

        public void StopAudio(SoundData sound)
        {
            foreach (AudioSource audioSource in _sourceList)
            {
                if (audioSource.clip == sound.Clip && audioSource.isPlaying)
                    audioSource.Stop();
            }
        }

        public void StopAudio(List<SoundData> soundList)
        {
            foreach (SoundData sound in soundList)
            {
                foreach (AudioSource audioSource in _sourceList)
                {
                    if (audioSource.clip == sound.Clip && audioSource.isPlaying)
                        audioSource.Stop();
                }
            }
        }

        public void SetSourceSettings(SoundData sound, AudioSource source, float spatialBlend = 1f)
        {
            source.clip = sound.Clip;
            source.volume = sound.Volume;
            source.pitch = sound.PitchVariation != 0 ? sound.Pitch * 1 + Random.Range(-sound.PitchVariation / 2, sound.PitchVariation / 2) : sound.Pitch;
            source.loop = sound.Loop;
            source.outputAudioMixerGroup = sound.mixerGroup;
            source.spatialBlend = sound.is3D ? 1 : 0;
            source.minDistance = sound.minDistance;
            source.maxDistance = sound.maxDistance;
        }

        private AudioSource InstantiateAudioSource(string name)
        {
            GameObject temp_audio = new(name);
            AudioSource newAudioSource = temp_audio.AddComponent<AudioSource>();

            _sourceList.Add(newAudioSource);

            temp_audio.transform.SetParent(transform);

            return newAudioSource;
        }

        private AudioSource GetOrCreateSource()
        {
            AudioSource targetSource = null;

            foreach (AudioSource audioSource in _sourceList)
            {
                if (!audioSource.isPlaying)
                {
                    targetSource = audioSource;
                }
            }

            if (_sourceList.Count <= 0 || !targetSource)
            {
                targetSource = InstantiateAudioSource(AudioSourceNamePrefix + _sourceList.Count);
            }

            return targetSource;
        }

#if UNITY_EDITOR
        [ContextMenu("Delete Prefs")]
        public void DeletePrefs()
        {
            _saveSystem.DeletePlayerPrefs();
        }
#endif
    }
}