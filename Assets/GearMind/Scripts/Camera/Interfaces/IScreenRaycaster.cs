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
        public RaycastHit2D? RaycastPhysics2D(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        );
        public RaycastHit2D[] RaycastPhysics2DAll(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        );
    }

    public interface IScreenRaycaster2D : IScreenRaycasterPhysics2D, IScreenRaycasterUI
    {
        public RaycastHit2D? RaycastPhysics2DStopAtUI(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        );
        public RaycastHit2D[] RaycastPhysics2DStopAtUIAll(
            Vector2 screenPosition,
            float distance = float.PositiveInfinity,
            LayerMask layerMask = default
        );
    }
}
