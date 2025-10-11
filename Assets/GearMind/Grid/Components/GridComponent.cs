using System;
using System.Collections.Generic;
using System.Linq;
using Assets.Utils.Runtime;
using EditorAttributes;
using Newtonsoft.Json;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public partial class GridComponent : MonoBehaviour, ISerializationCallbackReceiver
    {
        [SerializeField, Clamp(1, 1000, 1, 1000)]
        private Vector2Int _size = Vector2Int.one;

        [SerializeField, Clamp(0.01f, 100f, 0.01f, 100f)]
        private float _cellSize = 1f;

        [SerializeField]
        private GridCanvas _gridCanvas;

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

        public IReadOnlyDictionary<IGridObject, GridItem> Items => _items;

        public Vector3 WorldCenter => transform.position;
        public Vector2 WorldSize => (Vector2)Size * CellScale;
        public Vector2 WorldExtends => WorldSize / 2f;

        public Rect WordRect => new((Vector2)WorldCenter - WorldExtends, WorldSize);
        public Bounds WorldBounds => new(WorldCenter, WorldSize);

        public Grid Cells { get; private set; }

        private readonly Dictionary<IGridObject, GridItem> _items = new();

        public void Start() => OnGridChangedOrInit?.Invoke();

        public bool CanAddItem(IGridObject gridObject, Vector2Int position)
        {
            if (_items.ContainsKey(gridObject))
                return false;
            var gridItem = new GridItem(gridObject.Cells, position, gridObject);
            if (!Cells.IsItemInBounds(gridItem, position))
                return false;
            if (!gridObject.ValidateAt(position, Cells))
                return false;
            return true;
        }

        public bool AddItem(IGridObject gridObject, Vector2Int position)
        {
            if (gridObject is not MonoBehaviour monoGridObject)
                throw new ArgumentException("Grid object must be a MonoBehaviour");
            if (!CanAddItem(gridObject, position))
                return false;
            var additionalCells = gridObject.GetAdditionalCellsAt(position, Cells);
            var cells = gridObject.Cells.ConcatIfNotNull(additionalCells);
            var gridItem = new GridItem(cells, position, gridObject);
            if (!Cells.AddItem(gridItem, position))
                return false;
            _items.Add(gridObject, gridItem);
            // ? Should add validate other items before add this item
            PlaceMonoObject(monoGridObject, gridItem.Position);
            gridObject.AfterPlace(position, Cells);
            return true;
        }

        public IEnumerable<GridItem> RemoveItemRecursive(IGridObject target)
        {
            if (!_items.TryGetValue(target, out var gridItem))
                throw new ArgumentException("Grid object not found in items dictionary");
            var removed = new HashSet<GridItem>();
            RemoveItemRecursive(gridItem, removed);
            return removed;
        }

        private void RemoveItemRecursive(GridItem target, HashSet<GridItem> removed)
        {
            RemoveItemUnsave(target);
            removed.Add(target);

            while (true)
            {
                var validationFailure = GetFirstValidationFailure(target);
                if (validationFailure == null)
                    break;
                RemoveItemRecursive(validationFailure, removed);
            }
        }

        private GridItem GetFirstValidationFailure(GridItem item)
        {
            foreach (var cell in item.Cells)
            foreach (var record in Cells[item.Position + cell.Offset])
                if (!record.Item.Component.ValidateAt(record.Item.Position, Cells))
                    return record.Item;

            return null;
        }

        private void RemoveItemUnsave(GridItem gridItem)
        {
            gridItem.Component.BeforeRemove(Cells);
            _items.Remove(gridItem.Component);
            Cells.RemoveItem(gridItem);
        }

        public MonoBehaviour PrepareObject(MonoBehaviour gridObject)
        {
            gridObject.transform.localScale = CellScale * Vector3.one;
            return gridObject;
        }

        private void PlaceMonoObject(MonoBehaviour gridObject, Vector2Int position)
        {
            gridObject.transform.position = CellToWorld(position);
            gridObject.transform.SetParent(transform, true);
        }

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
    }
}
