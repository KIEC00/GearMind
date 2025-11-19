using UnityEngine;
using UnityEngine.SceneManagement;

namespace Assets.GearMind.Level
{
    public class LevelContext
    {
        public LevelData Level => GetLevelData(_levelIndex, _provider);
        public int LevelIndex => _levelIndex;
        public int LevelNumber => LevelIndex + 1;
        public bool IsLast => LevelIndex == _provider.Levels.Count - 1;
        public int Count => _provider.Levels.Count;
        public LevelContext Next => new(LevelIndex + 1, _provider);
        public int MenuSceneID => _provider.MenuSceneID;

        private readonly int _levelIndex;
        private readonly ILevelProvider _provider;

        public LevelContext(int levelIndex, ILevelProvider provider)
        {
            if (!IsValidIndex(levelIndex, provider))
            {
#if !UNITY_EDITOR
                Debug.LogError($"LevelProvider does not contains level with index {levelIndex}");
#endif
                levelIndex = int.MinValue;
            }

            _levelIndex = levelIndex;
            _provider = provider;
        }

        private static bool IsValidIndex(int levelIndex, ILevelProvider provider) =>
            levelIndex >= 0 || levelIndex >= provider.Levels.Count;

        private static LevelData GetLevelData(int levelIndex, ILevelProvider provider)
        {
#if UNITY_EDITOR
            if (!IsValidIndex(levelIndex, provider))
                return new(SceneManager.GetActiveScene().buildIndex);
#endif
            return provider.Levels[levelIndex];
        }
    }
}
