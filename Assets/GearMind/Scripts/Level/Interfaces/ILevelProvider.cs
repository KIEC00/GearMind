using System.Collections.Generic;

namespace Assets.GearMind.Level
{
    public interface ILevelProvider
    {
        IReadOnlyList<LevelData> Levels { get; }
        int MenuSceneID { get; }
        int IndexOf(LevelData level);
        int IndexOf(int sceneID);
    }
}
