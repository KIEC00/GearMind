using System;
using Assets.GearMind.Level;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public class LevelGoalTrigger : MonoBehaviour, ILevelGoalTrigger
    {
        public event Action Trigger;

        private void OnTriggerEnter2D(Collider2D collision) => Trigger?.Invoke();
    }
}
