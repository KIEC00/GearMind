using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public class FanVisual : MonoBehaviour
    {
        [SerializeField, Required]
        private Renderer _renderer;

        private Color _initialColor;

        private void Awake() => _initialColor = _renderer.material.color;

        public void OnDragStart() =>
            _renderer.material.color = _initialColor.WithAlpha(Constants.DRAG_ALPHA);

        public void OnDragEnd() => _renderer.material.color = _initialColor;

        public void SetDragError(bool isError) =>
            _renderer.material.color = isError
                ? Color.red.WithAlpha(Constants.DRAG_ALPHA)
                : _initialColor.WithAlpha(Constants.DRAG_ALPHA);
    }
}
