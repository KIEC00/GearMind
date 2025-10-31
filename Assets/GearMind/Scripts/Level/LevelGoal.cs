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
            GetComponent<Renderer>().material.color = Color.green.WithAlpha(0.1f);
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
