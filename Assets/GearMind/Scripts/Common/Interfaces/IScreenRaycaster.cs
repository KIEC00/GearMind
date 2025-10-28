using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Assets.GearMind.Common
{
    public interface IScreenRaycasterUI
    {
        public bool IsPointerOverUI(Vector2 screenPosition);
        public RaycastResult? RaycastUI(Vector2 screenPosition);
        public List<RaycastResult> RaycastUIAll(Vector2 screenPosition);
    }

    public interface IScreenRaycasterPhysics2D
    {
        public Vector2 ScreenToWorldPoint2D(Vector2 screenPosition);

        public Collider2D RaycastPhysics2D(Vector2 screenPosition, LayerMask layerMask);
        public Collider2D[] RaycastPhysics2DAll(Vector2 screenPosition, LayerMask layerMask);

        public Collider2D RaycastPhysics2D(Vector2 screenPosition) =>
            RaycastPhysics2D(screenPosition, -1);
        public Collider2D[] RaycastPhysics2DAll(Vector2 screenPosition) =>
            RaycastPhysics2DAll(screenPosition, -1);
    }

    public interface IScreenRaycaster2D : IScreenRaycasterPhysics2D, IScreenRaycasterUI
    {
        public Collider2D RaycastPhysics2DStopAtUI(Vector2 screenPosition, LayerMask layerMask);
        public Collider2D[] RaycastPhysics2DStopAtUIAll(
            Vector2 screenPosition,
            LayerMask layerMask
        );

        public Collider2D RaycastPhysics2DStopAtUI(Vector2 screenPosition) =>
            RaycastPhysics2DStopAtUI(screenPosition, -1);
        public Collider2D[] RaycastPhysics2DStopAtUIAll(Vector2 screenPosition) =>
            RaycastPhysics2DStopAtUIAll(screenPosition, -1);
    }
}
