using System;
using System.Collections.Generic;
using System.Linq;
using EditorAttributes;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public partial class GridComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        public event Action OnGridChangedOrInit;

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

        [SerializeField, Clamp(1, 1000, 1, 1000)]
        private Vector2Int _size = Vector2Int.one;

        [SerializeField, Clamp(0.01f, 100f, 0.01f, 100f)]
        private float _cellSize = 1f;

        [SerializeField]
        private GridCanvas _gridCanvas;

        public Vector3 WorldCenter => transform.position;
        public Vector2 WorldSize => (Vector2)Size * CellScale;
        public Vector2 WorldExtends => WorldSize / 2f;

        public Rect WordRect => new((Vector2)WorldCenter - WorldExtends, WorldSize);
        public Bounds WorldBounds => new(WorldCenter, WorldSize);

        public Grid Cells { get; private set; }

        private readonly Dictionary<IGridItemComponent, GridItem> _itemComponents = new();

        public void Start() => OnGridChangedOrInit?.Invoke();

        public bool AddItem(IGridItemComponent itemComponent, Vector2Int position)
        {
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, position, itemComponent);
            var added = Cells.AddItem(gridItem, position);
            if (added)
                _itemComponents.Add(itemComponent, gridItem);
            return added;
        }

        public bool AddItem(
            IGridItemComponent itemComponent,
            Vector2Int position,
            out IEnumerable<GridItem> attachedTo
        )
        {
            attachedTo = null;
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, position, itemComponent);
            var added = Cells.AddItem(gridItem, position, out attachedTo);
            if (added)
                _itemComponents.Add(itemComponent, gridItem);
            return added;
        }

        public bool CanAddItem(IGridItemComponent itemComponent, Vector2Int position)
        {
            if (_itemComponents.ContainsKey(itemComponent))
                return false;
            var gridItem = new GridItem(itemComponent.Cells, position, itemComponent);
            return Cells.CanAddItem(gridItem, position);
        }

        public IReadOnlyCollection<GridItem> RemoveItemsRecursive(IGridItemComponent itemComponent)
        {
            if (!_itemComponents.TryGetValue(itemComponent, out var gridItem))
                return null;
            var targets = Cells.RemoveItemsRecursive(gridItem);
            foreach (var target in targets)
                _itemComponents.Remove(target.Component);
            return targets;
        }

        public IReadOnlyCollection<GridItem> RemoveItemsRecursive(GridItem item)
        {
            var targets = Cells.RemoveItemsRecursive(item);
            foreach (var target in targets)
                _itemComponents.Remove(target.Component);
            return targets;
        }

        public GridItem GetSolidItemAt(Vector2Int position) =>
            Cells[position.x, position.y].GetSolidRecord()?.Item;

        public IEnumerable<GridItem> GetItemsAt(Vector2Int position) =>
            Cells[position.x, position.y].Select(r => r.Item);

        public void OnAfterDeserialize() => Cells = new Grid(Size.x, Size.y);

        public void OnBeforeSerialize() { }

        private void HandleUpdateGridSize(Vector2Int size)
        {
            _size = size;
            OnGridChangedOrInit?.Invoke();
        }

        private void HandleUpdateCellSize(float cellSize)
        {
            _cellSize = cellSize;
            OnGridChangedOrInit?.Invoke();
        }

        //Test
        public IEnumerable<GridItem> GetAllItems()
        {
            return _itemComponents.Values.ToList();
        }
    }
}
