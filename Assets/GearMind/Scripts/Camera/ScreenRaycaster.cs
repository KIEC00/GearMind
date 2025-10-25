using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.GearMind.Common
{
    public class ScreenRaycaster : IScreenRaycaster2D
    {
        private readonly ICameraProvider _cameraProvider;
        private readonly GraphicRaycaster _graphicRaycaster;
        private readonly EventSystem _eventSystem;

        public ScreenRaycaster(
            ICameraProvider cameraProvider,
            GraphicRaycaster graphicRaycaster,
            EventSystem eventSystem
        )
        {
            _cameraProvider = cameraProvider;
            _graphicRaycaster = graphicRaycaster;
            _eventSystem = eventSystem;
        }

        public bool IsPointerOverUI(Vector2 screenPosition) => RaycastUI(screenPosition).HasValue;

        public RaycastResult? RaycastUI(Vector2 screenPosition)
        {
            var results = RaycastUIAll(screenPosition);
            return results.Count > 0 ? results[0] : null;
        }

        public List<RaycastResult> RaycastUIAll(Vector2 screenPosition)
        {
            var pointerEventData = new PointerEventData(_eventSystem) { position = screenPosition };
            var results = new List<RaycastResult>();
            _graphicRaycaster.Raycast(pointerEventData, results);
            return results;
        }

        public RaycastHit2D? RaycastPhysics2D(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        )
        {
            var ray = _cameraProvider.Current.ScreenPointToRay(screenPosition);
            var hit = Physics2D.Raycast(ray.origin, ray.direction, distance, layerMask);
            return hit.collider != null ? hit : null;
        }

        public RaycastHit2D[] RaycastPhysics2DAll(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        )
        {
            var ray = _cameraProvider.Current.ScreenPointToRay(screenPosition);
            return Physics2D.RaycastAll(ray.origin, ray.direction, distance, layerMask);
        }

        public RaycastHit2D? RaycastPhysics2DStopAtUI(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        )
        {
            if (RaycastUI(screenPosition).HasValue)
                return null;
            return RaycastPhysics2D(screenPosition, distance, layerMask);
        }

        public RaycastHit2D[] RaycastPhysics2DStopAtUIAll(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        )
        {
            if (!RaycastUI(screenPosition).HasValue)
                return new RaycastHit2D[0];
            return RaycastPhysics2DAll(screenPosition, distance, layerMask);
        }
    }
}
