using UnityEngine;

namespace Assets.GearMind.Grid
{
    public partial class GridComponent
    {
        public Vector2Int? ScreenToCell(Vector2 screenPosition, Camera camera)
        {
            var planePosition = ScreenToPlane(screenPosition, camera);
            if (!planePosition.HasValue)
                return null;
            return WorldToCell(planePosition.Value);
        }

        public Vector3? ScreenToPlane(Vector2 screenPosition, Camera camera)
        {
            if (camera == null)
                return null;
            var ray = camera.ScreenPointToRay(screenPosition);
            var gridPlane = new Plane(Vector3.back, transform.position);
            if (gridPlane.Raycast(ray, out float enter))
                return ray.GetPoint(enter);
            return null;
        }

        public Vector2Int? WorldToCell(Vector3 worldPosition)
        {
            var gridExtends = WorldExtends;
            var center = WorldCenter;

            var cell = new Vector2Int(
                (int)Mathf.Floor(worldPosition.x - center.x + gridExtends.x / CellScale),
                (int)Mathf.Floor(worldPosition.y - center.y + gridExtends.y / CellScale)
            );

            return InBounds(cell) ? cell : null;
        }

        public Vector3 CellToWorld(Vector2Int cellPosition)
        {
            var cellExtents = 0.5f * CellScale;
            var gridExtends = WorldExtends;
            var localPosition = new Vector3(
                cellPosition.x * CellScale - gridExtends.x + cellExtents,
                cellPosition.y * CellScale - gridExtends.y + cellExtents
            );
            return WorldCenter + localPosition;
        }
    }
}
