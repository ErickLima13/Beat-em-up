using PainfulSmile.Runtime.Core;
using System.Collections.Generic;
using UnityEngine;

namespace PainfulSmile.Runtime.Systems.AudioSystem.Core
{
    public class AudioSaveSystem
    {
        private readonly AudioManager _manager;

        private readonly List<AudioReference> _audioPlayerPrefsReferences = new();

        public bool IsInitialized { get; private set; }

        public AudioSaveSystem(AudioManager audioManager)
        {
            _manager = audioManager;

            Initialize();
        }

        public void Initialize()
        {
            StorePlayerPrefsKeys();

            IsInitialized = true;
        }

        private void StorePlayerPrefsKeys()
        {
            if (_audioPlayerPrefsReferences.Count > 0)
            {
                return;
            }

            AudioReference masterReference = CreateReference(AudioSourceType.MASTER, PainfulSmileKeys.Audio.MasterAudioVolumeKey, PainfulSmileKeys.Audio.MasterAudioMuteKey);
            AudioReference musicReference = CreateReference(AudioSourceType.MUSIC, PainfulSmileKeys.Audio.MusicAudioVolumeKey, PainfulSmileKeys.Audio.MusicAudioMuteKey);
            AudioReference sfxReference = CreateReference(AudioSourceType.SFX, PainfulSmileKeys.Audio.SfxAudioVolumeKey, PainfulSmileKeys.Audio.SfxAudioMuteKey);

            _audioPlayerPrefsReferences.Add(masterReference);
            _audioPlayerPrefsReferences.Add(musicReference);
            _audioPlayerPrefsReferences.Add(sfxReference);
        }

        internal void SetDefaultMixerVolumes()
        {
            foreach (AudioReference audioReference in _audioPlayerPrefsReferences)
            {
                _manager.SetMixerVolume(audioReference.type, LoadVolumeValue(audioReference.type));
            }
        }

        internal void SetDefaultMixerMutes()
        {
            foreach (AudioReference audioReference in _audioPlayerPrefsReferences)
            {
                _manager.SetMute(audioReference.type, LoadMuteValue(audioReference.type), LoadVolumeValue(audioReference.type));
            }
        }

        internal float LoadVolumeValue(AudioSourceType audioType)
        {
            AudioReference audioReference = GetReference(audioType);

            if (audioReference == null)
            {
                return _manager.DefaultValue;
            }

            if (PlayerPrefs.HasKey(audioReference.key))
            {
                return PlayerPrefs.GetFloat(audioReference.key);
            }

            if (audioType == AudioSourceType.MASTER || audioType == AudioSourceType.MUSIC)
                return _manager.MaxValue;

            return _manager.DefaultValue;
        }

        internal bool LoadMuteValue(AudioSourceType audioType)
        {
            AudioReference audioReference = GetReference(audioType);

            if (audioReference == null)
            {
                return _manager.DefaultMutedValue;
            }

            if (PlayerPrefs.HasKey(audioReference.muteKey))
            {
                return PlayerPrefs.GetInt(audioReference.muteKey) == 1;
            }

            return _manager.DefaultMutedValue;
        }

        internal void SaveAudioValue(AudioSourceType audioType, float value)
        {
            AudioReference audioReference = GetReference(audioType);

            if (audioReference == null)
            {
                return;
            }

            PlayerPrefs.SetFloat(audioReference.key, value);
            PlayerPrefs.Save();
        }

        internal void SaveMuteValue(AudioSourceType audioType, bool value)
        {
            AudioReference audioReference = GetReference(audioType);

            if (audioReference == null)
            {
                return;
            }

            PlayerPrefs.SetInt(audioReference.muteKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        private AudioReference CreateReference(AudioSourceType type, string playerPrefsKey, string muteKey)
        {
            return new()
            {
                type = type,
                key = playerPrefsKey,
                muteKey = muteKey,
            };
        }

        private AudioReference GetReference(AudioSourceType type)
        {
            AudioReference audioReference = null;

            foreach (AudioReference reference in _audioPlayerPrefsReferences)
            {
                if (reference.type != type)
                    continue;

                audioReference = reference;
            }

            return audioReference;
        }

        internal void DeletePlayerPrefs()
        {
            foreach (AudioReference audioReference in _audioPlayerPrefsReferences)
            {
                PlayerPrefs.DeleteKey(audioReference.key);
                PlayerPrefs.DeleteKey(audioReference.muteKey);
            }
        }
    }
}
