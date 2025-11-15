using System;
using Assets.GearMind.Level;
using UnityEngine;

namespace Assets.GearMind.Instruments
{
    public class LevelGoalTrigger : MonoBehaviour, IGameplayObject, ILevelGoalTrigger
    {
        public event Action Trigger;

        [SerializeField]
        private Collider2D _collider;

        public void EnterEditMode() => _collider.isTrigger = false;

        public void EnterPlayMode() => _collider.isTrigger = true;

        private void OnTriggerEnter2D(Collider2D collision) => Trigger?.Invoke();
    }
}
