using Assets.GearMind.Objects.Utils;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public class BaseRigidbodyGamplayObject : MonoBehaviour, IGameplayObject
    {
        [SerializeField, Required]
        private Rigidbody2D _rigidbody;

        private RigitBody2DState _state;

        public void EnterEditMode() => _rigidbody.bodyType = RigidbodyType2D.Static;

        public void EnterPlayMode() => _rigidbody.bodyType = RigidbodyType2D.Dynamic;

        public void LoadState() => _rigidbody.SetState(_state);

        public void SaveState() => _state = _rigidbody.GetState();
    }
}
