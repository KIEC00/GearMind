using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class SettingsController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _closeSettingsButton;
        private VisualElement _settingsPanel;

        public bool IsVisible => _settingsPanel.style.display == DisplayStyle.Flex;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _settingsPanel = _doc.rootVisualElement;
            _closeSettingsButton = _settingsPanel.Q<Button>("CloseButton");
            if (_closeSettingsButton != null)
                _closeSettingsButton.clicked += CloseSettings;

            _settingsPanel.style.display = DisplayStyle.None;
        }

        private void OpenSettings()
        {
            _settingsPanel.style.display = DisplayStyle.Flex;
        }

        private void CloseSettings()
        {
            _settingsPanel.style.display = DisplayStyle.None;
        }

        public void Toggle()
        {
            if (IsVisible) CloseSettings();
            else OpenSettings();
        }

        private void OnDisable()
        {
            _closeSettingsButton.clicked -= CloseSettings;
        }
    }
}
