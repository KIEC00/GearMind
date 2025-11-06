using System.Collections.Generic;
using Assets.GearMind.Objects;
using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Test
{
    public class DefaultRigidObject
        : MonoBehaviour,
            IGameplayObject,
            IHaveState<Rigidbody2DState>,
            IDragAndDropTarget
    {
        private const float DRAG_ALPHA = 0.5f;

        [field: SerializeField]
        public bool IsDragable { get; set; } = false;

        [field:SerializeField, Required]
        public Renderer _renderer { get; private set; }

        private Color _initialColor;

        [field:SerializeField, Required]
        public Rigidbody2D _rigidbody { get; private set; }

        [field: SerializeField, Required]
        public Collider2D _collider { get; private set; }


        [field: SerializeField]
        public  ContactFilter2D _contactFilter { get; private set; }

        private readonly RaycastHit2D[] _hits = new RaycastHit2D[1];

        [Button]
        public virtual void EnterEditMode() => _rigidbody.bodyType = RigidbodyType2D.Kinematic;

        [Button]
        public virtual void EnterPlayMode() => _rigidbody.bodyType = RigidbodyType2D.Dynamic;

        public virtual Rigidbody2DState GetState() => _rigidbody.GetState();

        public virtual void SetState(Rigidbody2DState state) => _rigidbody.SetState(state);

        public void OnDragStart() => _renderer.material.color = _initialColor.WithAlpha(DRAG_ALPHA);

        public void OnDrag(Vector3 position) => transform.position = position;

        public void OnDragEnd() => _renderer.material.color = _initialColor.WithAlpha(1f);

        public virtual bool ValidatePlacement() =>
            _collider.Cast(Vector2.zero, _contactFilter, _hits, 0f) == 0; 

        public void SetError(bool isError)
        {
            _renderer.material.color = isError
                ? Color.red.WithAlpha(DRAG_ALPHA)
                : _initialColor.WithAlpha(DRAG_ALPHA);
        }

        private void Awake() => _initialColor = _renderer.material.color;

        
    }
}
