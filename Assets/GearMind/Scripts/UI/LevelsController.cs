using System.Collections.Generic;
using Assets.GearMind.Level;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using VContainer;

namespace Assets.GearMind.Scripts.UI
{
    [RequireComponent(typeof(UIDocument))]
    public class LevelsController : MonoBehaviour
    {
        [SerializeField]
        private VisualTreeAsset _levelButtonAsset;

        private UIDocument _doc;
        private Button _closeLevelsMenuButton;
        private VisualElement _levelsPanel;

        public bool IsVisible => _levelsPanel.style.display == DisplayStyle.Flex;

        [Inject]
        public void Construct(ILevelProvider levelProvider)
        {
            _doc = GetComponent<UIDocument>();

            CreateButtons(levelProvider.Levels);

            _levelsPanel = _doc.rootVisualElement;
            _closeLevelsMenuButton = _doc.rootVisualElement.Q<Button>("CloseButton");

            if (_closeLevelsMenuButton != null)
                _closeLevelsMenuButton.clicked += CloseLevelsMenu;

            _levelsPanel.style.display = DisplayStyle.None;
        }

        private void CreateButtons(IReadOnlyList<LevelData> levels)
        {
            var levelsContainer = _doc.rootVisualElement.Q("LevelsContainer");
            for (int i = 0; i < levels.Count; i++)
            {
                var level = levels[i];

                var buttonTree = _levelButtonAsset.CloneTree();

                var button = buttonTree.Q<Button>(className: "level-button");
                button.text = (i + 1).ToString();
                button.clicked += () => LoadLevel(level);

                levelsContainer.Add(buttonTree);
            }
        }

        private void OpenLevelsMenu() => _levelsPanel.style.display = DisplayStyle.Flex;

        private void CloseLevelsMenu() => _levelsPanel.style.display = DisplayStyle.None;

        public void Toggle()
        {
            if (IsVisible)
                CloseLevelsMenu();
            else
                OpenLevelsMenu();
            if (IsVisible)
                CloseLevelsMenu();
            else
                OpenLevelsMenu();
        }

        private void LoadLevel(LevelData level) => SceneManager.LoadScene(level.SceneID);

        private void OnDisable()
        {
            _closeLevelsMenuButton.clicked -= CloseLevelsMenu;
        }
    }
}
