using Assets.GearMind.Level;
using Assets.GearMind.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class InterfaceContoller : MonoBehaviour
    {
        [SerializeField]
        private NextLevelController _nextLevelController;

        [SerializeField]
        private SettingsController _settingsController;

        private UIManager _uiModeManager;

        private UIDocument _doc;
        private Button _startButton;
        private Button _reloadSceneButton;
        private Button _settingsButton;

        private VisualElement _playIcon;
        private VisualElement _editIcon;

        private LevelContext _levelContext;

        [Inject]
        public void Construct(UIManager uiModeManager, LevelContext levelContext)
        {
            _doc = GetComponent<UIDocument>();
            _uiModeManager = uiModeManager;
            _uiModeManager.OnLevelPassed += OnLevelPassed;
            _levelContext = levelContext;
        }

        private void OnEnable()
        {
            _startButton = _doc.rootVisualElement.Q<Button>("Start");
            _startButton.clicked += TogglePlayMode;

            _playIcon = _startButton.Q<VisualElement>("PlayIcon");
            _editIcon = _startButton.Q<VisualElement>("EditIcon");

            _reloadSceneButton = _doc.rootVisualElement.Q<Button>("Reload");
            _reloadSceneButton.clicked += ReloadScene;

            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _settingsButton.clicked += SettingsClicked;

            UpdateStartButtonText();
        }

        private void TogglePlayMode()
        {
            _uiModeManager.ToggleMode();

            UpdateStartButtonText();
        }

        private void UpdateStartButtonText()
        {
            if (_playIcon == null || _editIcon == null)
                return;

            _playIcon.style.display = _uiModeManager.IsEditMode
                ? DisplayStyle.Flex
                : DisplayStyle.None;
            _editIcon.style.display = _uiModeManager.IsSimulateMode
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        private void SettingsClicked() => _settingsController.Toggle();

        private void ReloadScene() => SceneManager.LoadScene(_levelContext.Level.SceneID);

        private void OnLevelPassed() => _nextLevelController.ShowNextLevelPanel();

        private void OnDisable()
        {
            _startButton.clicked -= TogglePlayMode;
            _settingsButton.clicked -= SettingsClicked;
            _reloadSceneButton.clicked -= ReloadScene;
            _uiModeManager.OnLevelPassed -= OnLevelPassed;
        }
    }
}
