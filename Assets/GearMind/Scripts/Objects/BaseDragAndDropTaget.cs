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

        private Color _initialColor;

        private void Awake() => _initialColor = _renderer.material.color;

        public void OnDragStart() => _renderer.material.color = _initialColor.WithAlpha(DRAG_ALPHA);

        public void OnDrag(Vector3 position) => transform.position = position;

        public void OnDragEnd() => _renderer.material.color = _initialColor.WithAlpha(1f);

        public bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn)
        {
            dependsOn = null;
            return true;
        }

        public void SetError(bool isError)
        {
            _renderer.material.color = isError
                ? Color.red.WithAlpha(DRAG_ALPHA)
                : _initialColor.WithAlpha(DRAG_ALPHA);
        }
    }
}
