using System;
using UnityEngine.SceneManagement;

namespace Assets.GearMind.Level
{
    public class LevelManager
    {
        private LevelContext _context;

        public event Action OnLevelCompleted;

        public LevelManager(LevelContext context)
        {
            _context = context;
        }

        public void CompleteLevel()
        {
            OnLevelCompleted.Invoke();
        }

        public void LoadNextLevel()
        {
            var id = _context.IsLast ? _context.MenuSceneID : _context.Next.Level.SceneID;
            SceneManager.LoadScene(id);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(_context.Level.SceneID);
        }
    }
}
