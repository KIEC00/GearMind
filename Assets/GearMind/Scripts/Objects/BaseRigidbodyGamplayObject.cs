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

        public virtual void EnterEditMode() => _rigidbody.simulated = false;

        public virtual void EnterPlayMode() => _rigidbody.simulated = true;

        public virtual Rigidbody2DState GetState() => _rigidbody.GetState();

        public virtual void SetState(Rigidbody2DState state) => _rigidbody.SetState(state);
    }
}
