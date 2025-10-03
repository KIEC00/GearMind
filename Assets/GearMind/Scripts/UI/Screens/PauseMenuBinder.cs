using UnityEngine;
using UnityEngine.UI;

namespace Assets.GearMind.Scripts.UI.Screens
{
    public class PauseMenuBinder : WindowBinder<PauseMenuViewModel>
    {
        [SerializeField] private Button _resume;
        [SerializeField] private Button _settings;

        private void OnEnable()
        {
            _resume.onClick.AddListener(ResumeButtonClicked);
            _settings.onClick.AddListener(SettingsButtonClicked);
        }

        private void OnDisable()
        {
            _resume.onClick.RemoveListener(ResumeButtonClicked);
            _settings.onClick.RemoveListener(SettingsButtonClicked);
        }

        private void ResumeButtonClicked()
        {
           //
        }

        private void SettingsButtonClicked()
        {
            ViewModel.RequestOpenSettings();
        }

    }
}