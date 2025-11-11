using System;

namespace Assets.GearMind.Level
{
    public class LevelContext
    {
        public LevelData Level => _provider.Levels[LevelIndex];
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
            if (levelIndex < 0 || levelIndex >= provider.Levels.Count)
                throw new ArgumentException("Invalid level index");

            _levelIndex = levelIndex;
            _provider = provider;
        }
    }
}
