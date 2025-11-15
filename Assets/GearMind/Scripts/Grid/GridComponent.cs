using System;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    [ExecuteAlways]
    public partial class GridComponent : MonoBehaviour
    {
        [SerializeField, Clamp(1, 1000, 1, 1000)]
        [OnValueChanged(nameof(TriggerChangeEvent))]
        private Vector2Int _size = Vector2Int.one;

        [SerializeField, Clamp(0.01f, 100f, 0.01f, 100f)]
        [OnValueChanged(nameof(TriggerChangeEvent))]
        private float _cellSize = 1f;

        public event Action OnGridChangedOrInit;

        public Vector2Int Size => _size;
        public float CellScale => _cellSize;

        public Vector3 WorldCenter => transform.position;
        public Vector2 WorldSize => (Vector2)Size * CellScale;
        public Vector2 WorldExtends => WorldSize / 2f;

        public Rect WordRect => new((Vector2)WorldCenter - WorldExtends, WorldSize);
        public Bounds WorldBounds => new(WorldCenter, WorldSize);

        public void Start() => OnGridChangedOrInit?.Invoke();

        public bool InBounds(Vector2Int position) =>
            position.x >= 0 && position.x < Size.x && position.y >= 0 && position.y < Size.y;

        public bool InBounds(Vector2 position) => WordRect.Contains(position);

        public bool InBounds(Rect rect) => WordRect.Contains(rect);

        public bool InBounds(Bounds bounds) => InBounds(bounds.ToRect());

        [Button("TriggerChangeEvent")]
        public void TriggerChangeEvent() => OnGridChangedOrInit?.Invoke();
    }
}
