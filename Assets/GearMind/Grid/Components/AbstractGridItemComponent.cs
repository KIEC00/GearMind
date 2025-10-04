using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public abstract class AbstractGridItemComponent : MonoBehaviour, IGridItemComponent
    {
        public abstract IEnumerable<Cell> Cells { get; }

        public abstract bool Dynamic { get; }

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
                var cellPosition = position + new Vector3(cell.Position.x, cell.Position.y, 0);
                Gizmos.color = cell.IsSolid ? Color.green : Color.yellow;
                Gizmos.DrawCube(cellPosition, Vector2.one);
            }
        }
#endif
    }
}
