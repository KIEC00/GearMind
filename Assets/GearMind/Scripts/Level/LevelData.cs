using System;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Level
{
    [Serializable]
    public struct LevelData
    {
        public readonly int SceneID => _scene;
        public readonly TutorialDataSO TutorialData => _tutorialData;
        public readonly bool HasTutorial => _tutorialData != null;

        [SerializeField, SceneDropdown]
        private int _scene;

        [SerializeField]
        private TutorialDataSO _tutorialData;

        public LevelData(int sceneID)
        {
            _scene = sceneID;
            _tutorialData = null;
        }
    }
}
