using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class InterfaceContoller : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _startButton;
        private Button _reloadSceneButton;
        private Button _settingsButton;

        [SerializeField]
        private SettingsController _settingsController;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();

        }


        private void OnEnable()
        {
            _startButton = _doc.rootVisualElement.Q<Button>("Start");
            //_startButton.clicked += ;

            _reloadSceneButton = _doc.rootVisualElement.Q<Button>("Reload");
            //_reloadSceneButton.clicked += ;

            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _settingsButton.clicked += SettingsClicked;
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
