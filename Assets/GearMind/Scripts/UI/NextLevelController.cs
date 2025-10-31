using UnityEngine;
using UnityEngine.UIElements;
using Assets.GearMind.Level;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    public class NextLevelController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _nextLevelButton;
        private VisualElement _nextLevelPanel;
        private LevelManager _levelManager;

        [Inject]
        public void Construct(LevelManager levelManager)
        {
            _levelManager = levelManager;
            _levelManager.OnLevelCompleted += ShowNextLevelPanel;
        }

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _nextLevelPanel = _doc.rootVisualElement;
            _nextLevelButton = _doc.rootVisualElement.Q<Button>("ExitToNextLevelButton");
            _nextLevelPanel.style.display = DisplayStyle.None;
            _nextLevelButton.clicked += HandleNextLevelClick;
        }

        private void ShowNextLevelPanel()
        {
            _nextLevelPanel.style.display = DisplayStyle.Flex;

        }

        private void HandleNextLevelClick()
        {
            _levelManager.LoadNextLevel();
        }

        private void OnDisable()
        {
            _nextLevelButton.clicked -= HandleNextLevelClick;
        }

        private void OnDestroy()
        {
            _levelManager.OnLevelCompleted -= ShowNextLevelPanel;
        }
    }
}