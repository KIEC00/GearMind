using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Assets.GearMind.Grid.Components
{
#if UNITY_EDITOR
    public partial class GridComponent
    {
        public Action DebugOnChangeTransform;

        [Header("Debug")]
        [SerializeField]
        private bool _debugShowAlways = true;

        [SerializeField]
        private bool _debugShowGrid = true;

        [SerializeField]
        private bool _debugGridMouseOver = true;

        public void OnValidate() => OnGridChangedOrInit?.Invoke();

        private void OnDrawGizmos()
        {
            if (!_debugShowAlways && !UnityEditor.Selection.Contains(gameObject))
                return;
            if (_debugShowGrid)
                GizmosDrawGrid(Color.green, Color.red);
            if (_debugGridMouseOver && Application.isPlaying)
            {
                var cell = ScreenToCell(Mouse.current.position.ReadValue(), Camera.main);
                if (cell.HasValue)
                    GizmosDrawCell(cell.Value, true, Color.yellow);
            }
        }

        private void GizmosDrawCell(Vector2Int cell, bool fill, Color color)
        {
            var cellSize = new Vector3(CellScale, CellScale, 0);
            var cellCenter = CellToWorld(cell);
            Gizmos.color = color;
            if (fill)
                Gizmos.DrawCube(cellCenter, cellSize);
            else
                Gizmos.DrawWireCube(cellCenter, cellSize);
        }

        private void GizmosDrawGrid(Color cellColor, Color boundsColor)
        {
            Gizmos.color = cellColor;
            for (int x = 0; x < Size.x; x++)
            for (int y = 0; y < Size.y; y++)
                GizmosDrawCell(new Vector2Int(x, y), false, cellColor);
            Gizmos.color = boundsColor;
            Gizmos.DrawWireCube(WorldBounds.center, WorldBounds.size);
        }
    }
#endif
}
