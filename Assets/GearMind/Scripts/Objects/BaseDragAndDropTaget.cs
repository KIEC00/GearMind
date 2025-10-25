using System.Collections.Generic;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public class BaseDragAndDropTaget : MonoBehaviour, IDragAndDropTarget
    {
        private const float DRAG_ALPHA = 0.5f;

        [field: SerializeField]
        public bool IsDragable { get; set; } = false;

        [SerializeField, Required]
        private Renderer _renderer;

        public virtual void OnDragStart() =>
            _renderer.material.color = _renderer.material.color.WithAlpha(DRAG_ALPHA);

        public void OnDragPerform(Vector3 position) => transform.position = position;

        public virtual void OnDragEnd() =>
            _renderer.material.color = _renderer.material.color.WithAlpha(1f);

        public virtual bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn)
        {
            dependsOn = null;
            return true;
        }
    }
}
