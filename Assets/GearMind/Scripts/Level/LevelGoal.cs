using Assets.Utils.Runtime;
using UnityEngine;
using VContainer;

namespace Assets.GearMind.Level
{
    [RequireComponent(typeof(Renderer))]
    public class LevelGoal : MonoBehaviour
    {
        private LevelManager _manager;

        [Inject]
        public void Construct(LevelManager manager)
        {
            _manager = manager;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Ball"))
            {
                _manager.CompleteLevel();
            }
        }
    }
}
