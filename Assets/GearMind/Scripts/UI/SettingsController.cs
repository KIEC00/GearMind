using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class SettingsController : MonoBehaviour
    {
        public enum Context
        {
            MainMenu,
            Gameplay
        }

        private UIDocument _doc;
        private Button _closeSettingsButton;
        private Button _exitToMainMenuButton;
        private VisualElement _settingsPanel;

        public Context CurrentContext { get; private set; }

        public bool IsVisible => _settingsPanel.style.display == DisplayStyle.Flex;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _settingsPanel = _doc.rootVisualElement;
            _closeSettingsButton = _settingsPanel.Q<Button>("CloseButton");
            _exitToMainMenuButton = _settingsPanel.Q<Button>("ExitToMainMenuButton");

            if (_closeSettingsButton != null)
                _closeSettingsButton.clicked += CloseSettings;

            if (_exitToMainMenuButton != null)
                _exitToMainMenuButton.clicked += ExitToMainMenu;

            _settingsPanel.style.display = DisplayStyle.None;

            SetContext(Context.Gameplay);

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

        private void ExitToMainMenu()
        {
            SceneManager.LoadScene("MainMenu");
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
            _exitToMainMenuButton.clicked -= ExitToMainMenu;
        }
    }
}
