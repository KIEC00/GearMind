using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class SettingsAudioController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _musicToggleButton;
        private Button _soundToggleButton;


        public bool IsMusicOn { get; private set; } = true;
        public bool IsSoundOn { get; private set; } = true;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            var root = _doc.rootVisualElement;
            _musicToggleButton = root.Q<Button>("MusicToggleSwitch");
            _soundToggleButton = root.Q<Button>("SoundToggleSwitch");

            _musicToggleButton.clicked += ToggleMusic;
            _soundToggleButton.clicked += ToggleSound;

            UpdateToggleState(_musicToggleButton, IsMusicOn);
            UpdateToggleState(_soundToggleButton, IsSoundOn);
        }

        private void ToggleMusic()
        {
            IsMusicOn = !IsMusicOn;
            UpdateToggleState(_musicToggleButton, IsMusicOn);
        }

        private void ToggleSound()
        {
            IsSoundOn = !IsSoundOn;
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



        public void SetMusicState(bool isOn)
        {
            IsMusicOn = isOn;
            UpdateToggleState(_musicToggleButton, isOn);
        }

        public void SetSoundState(bool isOn)
        {
            IsSoundOn = isOn;
            UpdateToggleState(_soundToggleButton, isOn);
        }

        private void OnDisable()
        {
            _musicToggleButton.clicked -= ToggleMusic;
            _soundToggleButton.clicked -= ToggleSound;
        }   
    }
}