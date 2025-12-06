using Assets.GearMind.Level;
using Assets.GearMind.Storage.Endpoints;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class MenuController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _playButton;
        private Button _exitButton;
        private Button _levelsButton;
        private Button _settingsButton;

        [SerializeField]
        private MainMenuSettingsController _settingsController;

        [SerializeField]
        private LevelsController _levelsController;

        private ILevelProvider _levelProvider;
        private PassedLevelsIdsSet _passedLevels;

        [Inject]
        public void Construct(
            ILevelProvider levelProvider,
            LevelProgressEndpoint levelProgressEndpoint
        )
        {
            _levelProvider = levelProvider;
            _passedLevels = levelProgressEndpoint.Load();
        }

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();

            _playButton = _doc.rootVisualElement.Q<Button>("Play");
            _playButton.clicked += PlayClicked;

            _exitButton = _doc.rootVisualElement.Q<Button>("Exit");
            _exitButton.clicked += ExitClicked;

            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _settingsButton.clicked += SettingsClicked;

            _levelsButton = _doc.rootVisualElement.Q<Button>("Levels");
            _levelsButton.clicked += LevelsClicked;

            _settingsController.SetContext(MainMenuSettingsController.Context.MainMenu);
        }

        private void PlayClicked() => LoadLastLevel();

        private void ExitClicked() => ExitGame();

        private void SettingsClicked() => _settingsController.Toggle();

        private void LevelsClicked() => _levelsController.Toggle();

        private void LoadLastLevel()
        {
            foreach (var level in _levelProvider.Levels)
            {
                if (_passedLevels.Contains(level.SceneID))
                    continue;
                SceneManager.LoadScene(level.SceneID);
                return;
            }
            if (_levelProvider.Levels.Count > 0)
                SceneManager.LoadScene(_levelProvider.Levels[0].SceneID);
        }

        public void ExitGame() => Application.Quit();

        private void OnDisable()
        {
            _playButton.clicked -= PlayClicked;
            _exitButton.clicked -= ExitClicked;
            _settingsButton.clicked -= SettingsClicked;
            _levelsButton.clicked -= LevelsClicked;
        }
    }
}
