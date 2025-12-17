using System;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Level
{
    [Serializable]
    public struct LevelData
    {
        [SerializeField, SceneDropdown]
        private int _scene;

        [SerializeField]
        private bool _hasTutorial;

        [SerializeField]
        private TutorialDataSO _tutorialData;
        public LevelData(int sceneID)
        {
            _scene = sceneID;
            _hasTutorial = false;
            _tutorialData = null;
        }

        public int SceneID => _scene;
        public bool HasTutorial => _hasTutorial;
        public TutorialDataSO TutorialData => _tutorialData;
    }
}
