using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Common
{
    public class FOVControlComponent : MonoBehaviour
    {
        const float MIN_FOV = 1f;
        const float MAX_FOV = 179f;

        public float FOV
        {
            get => _camera.fieldOfView;
            set => SetFov(Mathf.Clamp(value, MIN_FOV, MAX_FOV));
        }

        [SerializeField, Required]
        private Camera _camera;

        [SerializeField]
        private float _targetDepth;

        private void SetFov(float targetFov)
        {
            var currentFov = _camera.fieldOfView;

            var transform = _camera.transform;
            var forward = transform.forward;
            var position = transform.position;

            var currentTg = Mathf.Tan(Mathf.Deg2Rad * currentFov * 0.5f);
            var targetTg = Mathf.Tan(Mathf.Deg2Rad * targetFov * 0.5f);
            var scale = currentTg / targetTg;

            var newTargetDepth = _targetDepth * scale;
            var moveBy = newTargetDepth - _targetDepth;
            position -= forward * moveBy;

            transform.position = position;
            _camera.fieldOfView = targetFov;
            _targetDepth = newTargetDepth;
        }

#if UNITY_EDITOR
        [SerializeField, OnValueChanged(nameof(EditorSetFov)), Clamp(MIN_FOV, MAX_FOV)]
        private float _fov;

        private void EditorSetFov() => SetFov(_fov);

        private void OnDrawGizmosSelected()
        {
            if (_camera == null)
                return;
            var transform = _camera.transform;
            var forward = transform.forward;
            transform.GetPositionAndRotation(out var position, out var rotation);
            var depthPos = position + forward * _targetDepth;
            var rect = _camera.CalculateSize(_targetDepth);
            Gizmos.color = Color.blueViolet;
            GizmosUtils.DrawWireCube(depthPos, rect, rotation);
        }

        private void OnValidate()
        {
            if (!_camera)
                _camera = GetComponentInChildren<Camera>();
            if (!_camera)
                return;
        }
#endif
    }
}
