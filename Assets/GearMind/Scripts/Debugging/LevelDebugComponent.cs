using Assets.GearMind.Level;
using EditorAttributes;
using UnityEngine;
using VContainer;

namespace Assets.GearMind.Debugging
{
    public class LevelDebugComponent : MonoBehaviour
    {
#if UNITY_EDITOR
        [Inject]
        private LevelStateMachine _levelStateMachine;

        [Header("Level state")]
        [SerializeField]
        private Void _levelStateVoid;

        [Button]
        private void EnterEditMode() => _levelStateMachine.TransitionTo(LevelState.Edit);

        [Button]
        private void EnterSimulationMode() => _levelStateMachine.TransitionTo(LevelState.Simulate);

        [Button]
        private void Continue() => _levelStateMachine.Continue();

        [Button]
        private void Pause() => _levelStateMachine.Pause();
#endif
    }
}
