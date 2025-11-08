using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class LevelsController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _closeLevelsMenuButton;
        private VisualElement _levelsPanel;

        private Dictionary<string, string> _levelScenes = new Dictionary<string, string>
        {
            { "1", "SceneTemplate" },
            { "2", "Level2" }
        };

        public bool IsVisible => _levelsPanel.style.display == DisplayStyle.Flex;


        private void Awake()
        {
            _doc = GetComponent<UIDocument>();
        }

        private void OnEnable()
        {
            _levelsPanel = _doc.rootVisualElement;
            _closeLevelsMenuButton = _doc.rootVisualElement.Q<Button>("CloseButton");

            if (_closeLevelsMenuButton != null)
                _closeLevelsMenuButton.clicked += CloseLevelsMenu;

            var levelButtons = _doc.rootVisualElement.Query<Button>(className: "level-button").ToList();
            foreach (var button in levelButtons)
            {
                var levelText = button.text;
                button.clicked += () => LoadLevel(levelText);
            }

            _levelsPanel.style.display = DisplayStyle.None;
        }

        private void OpenLevelsMenu()
        {
            _levelsPanel.style.display = DisplayStyle.Flex;
        }

        private void CloseLevelsMenu()
        {
            _levelsPanel.style.display = DisplayStyle.None;
        }

        public void Toggle()
        {
            if (IsVisible) CloseLevelsMenu();
            else OpenLevelsMenu();
        }

        private void LoadLevel(string levelNumber)
        {
            if (_levelScenes.ContainsKey(levelNumber))
            {
                var sceneName = _levelScenes[levelNumber];
                SceneManager.LoadScene(sceneName);
            }
            else return;
        }

        private void OnDisable()
        {
            _closeLevelsMenuButton.clicked -= CloseLevelsMenu;
        }
    }
}
