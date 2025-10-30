using Assets.GearMind.Level;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    public class InterfaceContoller : MonoBehaviour
    {
        private LevelStateMachine _levelStateMachine;
        private LevelManager _levelManager;

        private UIDocument _doc;
        private Button _startButton;
        private Button _reloadSceneButton;
        private Button _settingsButton;

        [SerializeField]
        private SettingsController _settingsController;

        [Inject]
        public void Construct(LevelStateMachine levelStateMachine, LevelManager levelManager)
        {
            _levelStateMachine = levelStateMachine;
            _levelManager = levelManager;
        }

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _startButton = _doc.rootVisualElement.Q<Button>("Start");
            _startButton.clicked += TogglePlayMode;

            _reloadSceneButton = _doc.rootVisualElement.Q<Button>("Reload");
            _reloadSceneButton.clicked += ReloadScene;

            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _settingsButton.clicked += SettingsClicked;
        }

        private void TogglePlayMode()
        {
            _levelStateMachine.TransitionTo(
                _levelStateMachine.CurrentState == LevelState.Edit
                    ? LevelState.Simulate
                    : LevelState.Edit
            );
        }

        private void SettingsClicked()
        {
            _settingsController.Toggle();
        }

        private void ReloadScene()
        {
            _levelManager.RestartLevel();
        }

        private void OnDisable()
        {
            _startButton.clicked -= TogglePlayMode;
            _settingsButton.clicked -= SettingsClicked;
            _reloadSceneButton.clicked -= ReloadScene;
        }
    }
}
