using Assets.GearMind.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class NextLevelController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _nextLevelButton;
        private VisualElement _nextLevelPanel;

        private LevelContext _levelContext;

        [Inject]
        public void Construct(LevelContext levelContext)
        {
            _levelContext = levelContext;
        }

        public void ShowNextLevelPanel() => _nextLevelPanel.style.display = DisplayStyle.Flex;

        private void OnEnable()
        {
            _doc = GetComponent<UIDocument>();
            _nextLevelPanel = _doc.rootVisualElement;
            _nextLevelButton = _doc.rootVisualElement.Q<Button>("ExitToNextLevelButton");
            _nextLevelPanel.style.display = DisplayStyle.None;
            _nextLevelButton.clicked += HandleNextLevelClick;
        }

        private void HandleNextLevelClick()
        {
            var sceneID = _levelContext.IsLast
                ? _levelContext.MenuSceneID
                : _levelContext.Next.Level.SceneID;
            SceneManager.LoadScene(sceneID);
        }

        private void OnDisable() => _nextLevelButton.clicked -= HandleNextLevelClick;
    }
}
