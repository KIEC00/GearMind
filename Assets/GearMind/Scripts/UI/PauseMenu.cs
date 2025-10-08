using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GearMind.Scripts.UI
{
    public class PauseMenu : MonoBehaviour
    {
        public GameObject pauseMenu;
        public GameObject settingsMenuPrefab;
        private GameObject settingsMenuInstance;
        public static bool isPaused;

        public void Start()
        {
            pauseMenu.SetActive(false);
        }

        public void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                if (isPaused)
                {
                    ResumeGame();
                }
                else
                {
                    PauseGame();
                }
            }
        }

        public void PauseGame()
        {
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }

        public void ResumeGame()
        {
            pauseMenu.SetActive(false);


            for (int i = 1; i < pauseMenu.transform.childCount; i++) //
            {
                pauseMenu.transform.GetChild(i).gameObject.SetActive(false);
            }

            
            Time.timeScale = 1f;
            isPaused = false;
        }

        public void OpenSettings()
        {
            if (settingsMenuInstance == null)
            {
                settingsMenuInstance = Instantiate(settingsMenuPrefab, pauseMenu.transform);
            }
            else
            {
                settingsMenuInstance.SetActive(true);
            }
        }

        public void QuitGame()
        {
            Application.Quit();
        }
    }
}
