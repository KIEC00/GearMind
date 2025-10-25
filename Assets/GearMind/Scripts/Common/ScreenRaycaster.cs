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

        public Collider2D RaycastPhysics2D(Vector2 screenPosition, LayerMask layerMask)
        {
            var point = _cameraProvider.Current.ScreenToWorldPoint2D(screenPosition);
            return Physics2D.OverlapPoint(point, layerMask);
        }

        public Collider2D[] RaycastPhysics2DAll(Vector2 screenPosition, LayerMask layerMask)
        {
            var point = _cameraProvider.Current.ScreenToWorldPoint2D(screenPosition);
            return Physics2D.OverlapPointAll(point, layerMask);
        }

        public Collider2D RaycastPhysics2DStopAtUI(Vector2 screenPosition, LayerMask layerMask)
        {
            if (RaycastUI(screenPosition).HasValue)
                return null;
            return RaycastPhysics2D(screenPosition, layerMask);
        }

        public Collider2D[] RaycastPhysics2DStopAtUIAll(Vector2 screenPosition, LayerMask layerMask)
        {
            if (!RaycastUI(screenPosition).HasValue)
                return new Collider2D[0];
            return RaycastPhysics2DAll(screenPosition, layerMask);
        }
    }
}
