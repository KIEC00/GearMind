using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public class BaseRigidbodyGamplayObject
        : MonoBehaviour,
            IGameplayObject,
            IHaveState<Rigidbody2DState>
    {
        [SerializeField, Required]
        private Rigidbody2D _rigidbody;

        [Button]
        public virtual void EnterEditMode() => _rigidbody.bodyType = RigidbodyType2D.Static;

        [Button]
        public virtual void EnterPlayMode() => _rigidbody.bodyType = RigidbodyType2D.Dynamic;

        public virtual Rigidbody2DState GetState() => _rigidbody.GetState();

        public virtual void SetState(Rigidbody2DState state) => _rigidbody.SetState(state);
    }
}
