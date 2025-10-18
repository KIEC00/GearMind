using Assets.GearMind.Level;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    public class InterfaceContoller : MonoBehaviour
    {
        private LevelStateMachine _levelStateMachine;

        private UIDocument _doc;
        private Button _startButton;
        private Button _reloadSceneButton;
        private Button _settingsButton;

        [SerializeField]
        private SettingsController _settingsController;

        [Inject]
        public void Construct(LevelStateMachine levelStateMachine) =>
            _levelStateMachine = levelStateMachine;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _startButton = _doc.rootVisualElement.Q<Button>("Start");
            _startButton.clicked += TogglePlayMode;

            _reloadSceneButton = _doc.rootVisualElement.Q<Button>("Reload");
            //_reloadSceneButton.clicked += ;

            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _settingsButton.clicked += SettingsClicked;
        }

        private void TogglePlayMode()
        {
            // TODO: Make readonly event, prevent set mode directly
            _levelStateMachine.Mode =
                _levelStateMachine.Mode == LevelMode.Edit ? LevelMode.Play : LevelMode.Edit;
        }

        private void SettingsClicked()
        {
            _settingsController.Toggle();
        }

        private void OnDisable()
        {
            _settingsButton.clicked -= SettingsClicked;
        }
    }
}
