using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Audio;

namespace Assets.GearMind.Audio
{
    public class AudioVolumeControlComponent : MonoBehaviour, IAudioVolumeControl
    {
        public float this[AudioChannel channel]
        {
            get => GetChannelVolume(channel);
            set => SetChannelVolume(channel, value);
        }

        [field: SerializeField, Range(0, 1)]
        public float DefaultMusicVolume { get; private set; } = 0.3f;

        [field: SerializeField, Range(0, 1)]
        public float DefaultSFXVolume { get; private set; } = 0.3f;

        [SerializeField, Required]
        private AudioMixer _audioMixer;

        private void Start()
        {
            var musicVolume = GetChannelVolume(AudioChannel.Music, -1);
            var sfxVolume = GetChannelVolume(AudioChannel.SFX, -1);
            if (musicVolume == -1)
                SetChannelVolume(AudioChannel.Music, DefaultMusicVolume);
            else
                SetAudioMixerVolume(AudioChannel.Music, musicVolume);
            if (sfxVolume == -1)
                SetChannelVolume(AudioChannel.SFX, DefaultSFXVolume);
            else
                SetAudioMixerVolume(AudioChannel.SFX, sfxVolume);
        }

        private string GetVolumeKey(AudioChannel channel) =>
            AudioMixerKeys.Volume.GetValueOrDefault(channel)
            ?? throw new Exception($"Invalid channel key {channel}");

        private float GetChannelVolume(AudioChannel channel, float defaultValue = 0) =>
            PlayerPrefs.GetFloat(GetVolumeKey(channel), defaultValue);

        private void SetChannelVolume(AudioChannel channel, float value)
        {
            value = Mathf.Clamp01(value);
            var key = GetVolumeKey(channel);
            PlayerPrefs.SetFloat(key, value);
            SetAudioMixerVolume(key, value);
        }

        private void SetAudioMixerVolume(AudioChannel channel, float value) =>
            SetAudioMixerVolume(GetVolumeKey(channel), value);

        private void SetAudioMixerVolume(string key, float value) =>
            _audioMixer.SetFloat(key, value == 0f ? -80f : (Mathf.Log10(value) * 20));

#if UNITY_EDITOR
        [Header("Editor")]
        [SerializeField, OnValueChanged(nameof(_SetEditorMusic)), Range(0, 1)]
        private float _editorMusicVolume;

        private void _SetEditorMusic() => SetChannelVolume(AudioChannel.Music, _editorMusicVolume);

        [SerializeField, OnValueChanged(nameof(_SetEditorSound)), Range(0, 1)]
        private float _editorSFXVolume;

        private void _SetEditorSound() => SetChannelVolume(AudioChannel.SFX, _editorSFXVolume);
#endif
    }
}
