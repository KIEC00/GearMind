using System;
using Assets.GearMind.Custom.Input;
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
        public event Action<Direction> OnRotateButtonStarted;
        public event Action OnRotateButtonStoped;

        [SerializeField, Required]
        private NextLevelController _nextLevelController;

        [SerializeField, Required]
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

        public void OnLevelPassed() => _nextLevelController.ShowNextLevelPanel();

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

            _rotateLeftButton.RegisterCallback<MouseDownEvent>(HandleRotateLeftStart);
            _rotateLeftButton.RegisterCallback<MouseUpEvent>(HandleRotateLeftStop);
            _rotateRightButton.RegisterCallback<MouseDownEvent>(HandleRotateRightStart);
            _rotateRightButton.RegisterCallback<MouseUpEvent>(HandleRotateRightStop);

            _uiManager.OnLevelPassed += OnLevelPassed;
        }

        private void HandleRotateLeftStart(MouseDownEvent e) =>
            OnRotateButtonStarted?.Invoke(Direction.Clockwise);

        private void HandleRotateLeftStop(MouseUpEvent e) => OnRotateButtonStoped?.Invoke();

        private void HandleRotateRightStart(MouseDownEvent e) =>
            OnRotateButtonStarted?.Invoke(Direction.Counterclockwise);

        private void HandleRotateRightStop(MouseUpEvent e) => OnRotateButtonStoped?.Invoke();

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

            var rotationDisplay = DisplayStyle.None; //TODO
            //    // _levelType == LevelType.Rotation ? DisplayStyle.Flex : DisplayStyle.None;
            _rotateLeftButton.style.display = rotationDisplay;
            _rotateRightButton.style.display = rotationDisplay;
        }

        private void SettingsClicked() => _settingsController.Toggle();

        private void ReloadScene() => SceneManager.LoadScene(_levelContext.Level.SceneID);

        private void OnDisable()
        {
            _startButton.clicked -= TogglePlayMode;
            _settingsButton.clicked -= SettingsClicked;
            _reloadSceneButton.clicked -= ReloadScene;
            _uiManager.OnLevelPassed -= OnLevelPassed;

            _rotateLeftButton.UnregisterCallback<MouseDownEvent>(HandleRotateLeftStart);
            _rotateLeftButton.UnregisterCallback<MouseUpEvent>(HandleRotateLeftStop);
            _rotateRightButton.UnregisterCallback<MouseDownEvent>(HandleRotateRightStart);
            _rotateRightButton.UnregisterCallback<MouseUpEvent>(HandleRotateRightStop);
        }
    }

    public enum LevelType
    {
        Default,
        Rotation,
    }
}
