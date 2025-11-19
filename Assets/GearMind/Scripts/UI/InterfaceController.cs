using Assets.GearMind.Level;
using Assets.GearMind.UI;
using EditorAttributes;
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

        [SerializeField]
        private LevelType _levelType = LevelType.Default;

        private UIManager _uiManager;

        private UIDocument _doc;
        private Button _startButton;
        private Button _reloadSceneButton;
        private Button _settingsButton;
        private Button _rotateLeftButton;
        private Button _rotateRightButton;

        private VisualElement _playIcon;
        private VisualElement _editIcon;

        private LevelContext _levelContext;

        [Inject]
        public void Construct(UIManager uiManager, LevelContext levelContext)
        {
            _uiManager = uiManager;
            _levelContext = levelContext;
        }

        private void Awake() => BindDocument();

        private void BindDocument()
        {
            _doc = GetComponent<UIDocument>();
            _startButton = _doc.rootVisualElement.Q<Button>("Start");
            _playIcon = _startButton.Q<VisualElement>("PlayIcon");
            _editIcon = _startButton.Q<VisualElement>("EditIcon");
            _reloadSceneButton = _doc.rootVisualElement.Q<Button>("Reload");
            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _rotateLeftButton = _doc.rootVisualElement.Q<Button>("RotateLeft");
            _rotateRightButton = _doc.rootVisualElement.Q<Button>("RotateRight");
        }

        private void OnEnable()
        {
            UpdateButtonsForCurrentLevel();
            UpdateStartButtonText();

            _startButton.clicked += TogglePlayMode;
            _reloadSceneButton.clicked += ReloadScene;
            _settingsButton.clicked += SettingsClicked;

            _uiManager.OnLevelPassed += OnLevelPassed;
        }

        private void TogglePlayMode()
        {
            _uiManager.ToggleMode();
            UpdateStartButtonText();
        }

        private void UpdateStartButtonText()
        {
            _playIcon.style.display = _uiManager.IsEditMode ? DisplayStyle.Flex : DisplayStyle.None;
            _editIcon.style.display = _uiManager.IsSimulateMode
                ? DisplayStyle.Flex
                : DisplayStyle.None;
        }

        private void UpdateButtonsForCurrentLevel()
        {
            _startButton.style.display =
                _levelType == LevelType.Rotation ? DisplayStyle.None : DisplayStyle.Flex;

            var rotationDisplay =
                _levelType == LevelType.Rotation ? DisplayStyle.Flex : DisplayStyle.None;
            _rotateLeftButton.style.display = rotationDisplay;
            _rotateRightButton.style.display = rotationDisplay;
        }

        private void SettingsClicked() => _settingsController.Toggle();

        private void ReloadScene() => SceneManager.LoadScene(_levelContext.Level.SceneID);

        private void OnLevelPassed() => _nextLevelController.ShowNextLevelPanel();

        private void OnDisable()
        {
            _startButton.clicked -= TogglePlayMode;
            _settingsButton.clicked -= SettingsClicked;
            _reloadSceneButton.clicked -= ReloadScene;
            _uiManager.OnLevelPassed -= OnLevelPassed;
        }
    }

    public enum LevelType
    {
        Default,
        Rotation,
    }
}
