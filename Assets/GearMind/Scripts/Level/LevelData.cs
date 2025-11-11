using System;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Level
{
    [Serializable]
    public struct LevelData
    {
        public readonly int SceneID => _scene;

        [SerializeField, SceneDropdown]
        private int _scene;

        public LevelData(int sceneID) => _scene = sceneID;
    }
}
