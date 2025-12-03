using Assets.GearMind.Audio;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    public class SettingsAudioController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _musicToggleButton;
        private Button _soundToggleButton;

        private AudioVolumeControlComponent _volumeControl;

        [Inject]
        public void Construct(AudioVolumeControlComponent volumeControl)
        {
            _volumeControl = volumeControl;
        }

        public bool IsMusicOn { get; private set; }
        public bool IsSoundOn { get; private set; }

        private void OnEnable()
        {
            _doc = GetComponent<UIDocument>();
            var root = _doc.rootVisualElement;

            _musicToggleButton = root.Q<Button>("MusicToggleSwitch");
            _soundToggleButton = root.Q<Button>("SoundToggleSwitch");

            _musicToggleButton.clicked += ToggleMusic;
            _soundToggleButton.clicked += ToggleSound;

            IsMusicOn = _volumeControl[AudioChannel.Music] > 0;
            IsSoundOn = _volumeControl[AudioChannel.SFX] > 0;

            UpdateToggleState(_musicToggleButton, IsMusicOn);
            UpdateToggleState(_soundToggleButton, IsSoundOn);
        }

        private void ToggleMusic()
        {
            IsMusicOn = !IsMusicOn;
            _volumeControl[AudioChannel.Music] = IsMusicOn ? _volumeControl.DefaultMusicVolume : 0f;
            UpdateToggleState(_musicToggleButton, IsMusicOn);
        }

        private void ToggleSound()
        {
            IsSoundOn = !IsSoundOn;
            _volumeControl[AudioChannel.SFX] = IsSoundOn ? _volumeControl.DefaultSFXVolume : 0f;
            UpdateToggleState(_soundToggleButton, IsSoundOn);
        }

        private void UpdateToggleState(VisualElement toggleButton, bool isOn)
        {
            if (isOn)
            {
                toggleButton.AddToClassList("on");
                var toggleText = toggleButton.Q<Label>("ToggleText");
                toggleText.text = "On";
            }
            else
            {
                toggleButton.RemoveFromClassList("on");
                var toggleText = toggleButton.Q<Label>("ToggleText");
                toggleText.text = "Off";
            }
        }

        private void OnDisable()
        {
            _musicToggleButton.clicked -= ToggleMusic;
            _soundToggleButton.clicked -= ToggleSound;
        }
    }
}