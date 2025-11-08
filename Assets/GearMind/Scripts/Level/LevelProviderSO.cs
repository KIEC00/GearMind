using System;
using System.Collections.Generic;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Level
{
    [CreateAssetMenu(fileName = "Levels", menuName = "GearMind/LevelProvider", order = 0)]
    public class LevelProviderSO : ScriptableObject, ILevelProvider
    {
        public IReadOnlyList<LevelData> Levels => _levels;

        [field: SerializeField, SceneDropdown]
        public int MenuSceneID { get; private set; }

        public int IndexOf(LevelData level) => Array.IndexOf(_levels, level);

        public int IndexOf(int sceneID) => Array.FindIndex(_levels, l => l.SceneID == sceneID);

        [SerializeField, DataTable]
        private LevelData[] _levels;
    }
}
