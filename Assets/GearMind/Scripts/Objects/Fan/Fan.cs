using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public class Fan
        : MonoBehaviour,
            IGameplayObject,
            IHaveState<Rigidbody2DState>,
            IDragAndDropTarget
    {
        [field: SerializeField]
        public bool IsDragable { get; set; } = false;

        [SerializeField, Clamp(0, 30f, order = 1)]
        [OnValueChanged(nameof(UpdateEffector))]
        private float _distance = 3f;

        [SerializeField, Clamp(0, 100f, order = 1)]
        [OnValueChanged(nameof(UpdateEffector))]
        private float _force = 10f;

        [SerializeField]
        private ContactFilter2D _contactFilter;

        [SerializeField, Required]
        private Rigidbody2D _rigidbody;

        [SerializeField, Required]
        private Collider2D _collider;

        [SerializeField, Required]
        private FanEffector _fanEffector;

        [SerializeField, Required]
        private FanVisual _fanVisual;

        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];

        private void Awake() => UpdateEffector();

        private void Reset() => UpdateEffector();

        [Button]
        public virtual void EnterEditMode() => _fanEffector.Disable();

        [Button]
        public virtual void EnterPlayMode() => _fanEffector.Enable();

        public virtual Rigidbody2DState GetState() => _rigidbody.GetState();

        public virtual void SetState(Rigidbody2DState state) => _rigidbody.SetState(state);

        public void OnDragStart() => _fanVisual.OnDragStart();

        public void OnDrag(Vector3 position) => transform.position = position;

        public void OnDragEnd() => _fanVisual.OnDragEnd();

        public void SetError(bool isError) => _fanVisual.SetDragError(isError);

        public bool ValidatePlacement() =>
            _collider.Cast(Vector2.zero, _contactFilter, _hits, 0f) == 0;

        private void UpdateEffector()
        {
            _fanEffector.SetLength(_distance);
            _fanEffector.SetForce(_force);
        }
    }
}
