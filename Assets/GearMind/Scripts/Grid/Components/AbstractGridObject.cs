using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public abstract class AbstractGridObject : MonoBehaviour, IGridObject
    {
        public abstract IEnumerable<Cell> Cells { get; }

        public abstract bool ValidateAt(Vector2Int position, Grid grid);

        public abstract IEnumerable<Cell> GetAdditionalCellsAt(Vector2Int position, Grid grid);

        public abstract void AfterPlace(Vector2Int position, Grid grid);

        public abstract void BeforeRemove(Grid grid);

#if UNITY_EDITOR
        [Header("Debug")]
        [SerializeField]
        private bool _debugDrawGizmos = false;

        void OnDrawGizmos()
        {
            if (!_debugDrawGizmos)
                return;
            var position = transform.position;
            foreach (var cell in Cells)
            {
                var cellPosition = position + new Vector3(cell.Offset.x, cell.Offset.y, 0);
                Gizmos.color = cell.Type == CellType.Requirement ? Color.blue : Color.green;
                Gizmos.DrawCube(cellPosition, Vector2.one);
            }
        }

#endif
    }
}
