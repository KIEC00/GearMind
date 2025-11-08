using Assets.GearMind.Level;
using UnityEngine.SceneManagement;

namespace Assets.GearMind.Scripts.Level
{
    public class LevelContextProvider : ILevelContextProvider
    {
        public LevelContext Current => GetContext();

        private ILevelProvider _levelProvider;

        public LevelContextProvider(ILevelProvider levelProvider) => _levelProvider = levelProvider;

        private LevelContext GetContext()
        {
            var idx = _levelProvider.IndexOf(SceneManager.GetActiveScene().buildIndex);
            if (idx < 0)
                return null;
            return new(idx, _levelProvider);
        }
    }
}
