using System;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GearMind.Level
{
    public class LevelManager
    {
        private readonly LevelStateMachine _stateMachine;

        public event Action OnLevelCompleted;

        public LevelManager(LevelStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void CompleteLevel()
        {
            OnLevelCompleted.Invoke();
        }

        public void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            int nextSceneIndex = currentSceneIndex + 1;

            if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
                SceneManager.LoadScene(nextSceneIndex);
            else
                Debug.Log("Все уровни пройдены");
        }

        public void RestartLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex);
        }
    }
}
