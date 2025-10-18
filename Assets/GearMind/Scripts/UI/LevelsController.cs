using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class LevelsController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _closeLevelsMenuButton;
        private VisualElement _levelsPanel;

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

        private void OnDisable()
        {
            _closeLevelsMenuButton.clicked -= CloseLevelsMenu;
        }
    }
}
