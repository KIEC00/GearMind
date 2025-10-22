using System;
using EditorAttributes;
using UnityEngine;
using UnityEngine.Events;

namespace Assets.GearMind.Grid.Components
{
    [ExecuteAlways]
    public partial class GridComponent : MonoBehaviour
    {
        [SerializeField, Clamp(1, 1000, 1, 1000)]
        private Vector2Int _size = Vector2Int.one;

        [SerializeField, Clamp(0.01f, 100f, 0.01f, 100f)]
        private float _cellSize = 1f;

        [field: SerializeField]
        public UnityEvent<GridParams> OnGridChangedOrInit { get; private set; }

        public Vector2Int Size
        {
            get => _size;
            private set => HandleUpdateGridSize(value);
        }

        public float CellScale
        {
            get => _cellSize;
            private set => HandleUpdateCellSize(value);
        }

        public Vector3 WorldCenter => transform.position;
        public Vector2 WorldSize => (Vector2)Size * CellScale;
        public Vector2 WorldExtends => WorldSize / 2f;

        public Rect WordRect => new((Vector2)WorldCenter - WorldExtends, WorldSize);
        public Bounds WorldBounds => new(WorldCenter, WorldSize);
        public GridParams Params => new(WorldCenter, CellScale, Size);

        public void Start() => OnGridChangedOrInit?.Invoke(Params);

        public bool InBounds(Vector2Int position) =>
            position.x >= 0 && position.x < Size.x && position.y >= 0 && position.y < Size.y;

        public bool InBounds(Vector2 position) => WordRect.Contains(position);

        private void HandleUpdateGridSize(Vector2Int size)
        {
            _size = size;
            OnGridChangedOrInit?.Invoke(Params);
        }

        private void HandleUpdateCellSize(float cellSize)
        {
            _cellSize = cellSize;
            OnGridChangedOrInit?.Invoke(Params);
        }

        public class GridParams : IEquatable<GridParams>
        {
            public Vector3 WorldPosition;
            public float CellSize;
            public Vector2Int Size;

            public GridParams(Vector3 worldPosition, float cellSize, Vector2Int size)
            {
                WorldPosition = worldPosition;
                CellSize = cellSize;
                Size = size;
            }

            public bool Equals(GridParams other) =>
                WorldPosition.Equals(other.WorldPosition)
                && CellSize.Equals(other.CellSize)
                && Size.Equals(other.Size);
        }
    }
}
