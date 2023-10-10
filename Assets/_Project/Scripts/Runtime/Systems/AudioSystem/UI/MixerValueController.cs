using PainfulSmile.Runtime.Systems.AudioSystem.Core;
using UnityEngine;
using UnityEngine.UI;

namespace PainfulSmile.Runtime.Systems.AudioSystem.IU
{
    public class MixerValueController : MonoBehaviour
    {
        [Header("Mixer Values")]
        [SerializeField] private AudioSourceType _audioType;

        [Header("UI Elements")]
        [SerializeField] private Slider _volumeSlider;
        [SerializeField] private Toggle _muteToggle;

        private void OnEnable()
        {
            _volumeSlider.onValueChanged.AddListener(ChangeVolume);
            _muteToggle.onValueChanged.AddListener(Mute);

            _volumeSlider.minValue =  AudioManager.Instance.MinValue;
            _volumeSlider.maxValue = AudioManager.Instance.MaxValue;

            _volumeSlider.value = AudioManager.Instance.GetCurrentAudioValue(_audioType);

            Mute(AudioManager.Instance.GetMuteAudioValue(_audioType));
        }

        private void OnDisable()
        {
            _volumeSlider.onValueChanged.RemoveListener(ChangeVolume);
            _muteToggle.onValueChanged.RemoveListener(Mute);
        }

        public void ChangeVolume(float volume)
        {
            AudioManager.Instance.SetMixerVolume(_audioType, volume);
        }

        public void Mute(bool value)
        {
            _volumeSlider.interactable = !value;
            _muteToggle.isOn = value;

            float targetVolume = value ? AudioManager.Instance.MinValue : _volumeSlider.value;

            AudioManager.Instance.SetMute(_audioType, value, targetVolume);
        }
    }
}