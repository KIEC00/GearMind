using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

namespace Assets.GearMind.Scripts.UI
{
    public class MenuController : MonoBehaviour
    {
        private UIDocument _doc;
        private Button _playButton;
        private Button _exitButton;
        private Button _settingsButton;

        [SerializeField]
        private SettingsController _settingsController;

        private void Awake()
        {
            _doc = GetComponent<UIDocument>();

        }

        private void OnEnable()
        {
            _playButton = _doc.rootVisualElement.Q<Button>("Play");
            _playButton.clicked += PlayClicked;

            _exitButton = _doc.rootVisualElement.Q<Button>("Exit");
            _exitButton.clicked += ExitClicked;

            _settingsButton = _doc.rootVisualElement.Q<Button>("Settings");
            _settingsButton.clicked += SettingsClicked;

            _settingsController.SetContext(SettingsController.Context.MainMenu);

        }


        private void PlayClicked()
        {
            StartCoroutine(LoadScene());
        }

        private void ExitClicked()
        {
            ExitGame();
        }

        private void SettingsClicked()
        {
            _settingsController.Toggle();
        }

        private IEnumerator LoadScene()
        {
            AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("BaseScene");

            // Можно добавить загрузочный экран
            while (!asyncLoad.isDone)
            {
                float progress = Mathf.Clamp01(asyncLoad.progress / 0.9f);
                Debug.Log("Загрузка: " + (progress * 100) + "%");
                yield return null;
            }
        }

        public void ExitGame()
        {
            Application.Quit();
            Debug.Log("ExitGame");
        }

        private void OnDisable()
        {
            _playButton.clicked -= PlayClicked;
            _exitButton.clicked -= ExitClicked;
            _settingsButton.clicked -= SettingsClicked;
        }
    }
}
