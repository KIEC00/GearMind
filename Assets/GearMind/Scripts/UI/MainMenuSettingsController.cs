using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MainMenuSettingsController : MonoBehaviour
    {
        public enum Context
        {
            MainMenu,
            Gameplay,
        }

        private UIDocument _doc;
        private Button _closeSettingsButton;
        private Button _exitToMainMenuButton;
        private VisualElement _settingsPanel;

        public Context CurrentContext { get; private set; }

        public bool IsVisible => _settingsPanel.style.display == DisplayStyle.Flex;

        public void Awake()
        {
            _doc = GetComponent<UIDocument>();

            _settingsPanel = _doc.rootVisualElement;
            _closeSettingsButton = _settingsPanel.Q<Button>("CloseButton");
            _exitToMainMenuButton = _settingsPanel.Q<Button>("ExitToMainMenuButton");

            if (_closeSettingsButton != null)
                _closeSettingsButton.clicked += CloseSettings;

            _settingsPanel.style.display = DisplayStyle.None;

            SetContext(Context.MainMenu);
        }

        public void SetContext(Context context)
        {
            CurrentContext = context;

            if (_exitToMainMenuButton != null)
            {
                _exitToMainMenuButton.style.visibility =
                    (context == Context.Gameplay) ? Visibility.Visible : Visibility.Hidden;
            }
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
            if (IsVisible)
                CloseSettings();
            else
                OpenSettings();
        }

        private void OnDisable()
        {
            _closeSettingsButton.clicked -= CloseSettings;
        }
    }
}
