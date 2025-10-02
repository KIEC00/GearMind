using System;
using System.Collections.Generic;
using Assets.Utils.Runtime;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public partial class Grid
    {
        public IReadOnlyTable<IReadOnlyGridCell> Cells => (IReadOnlyTable<IReadOnlyGridCell>)_cells;
        public IEnumerable<GridItem> Items => _itemPositions.Keys;
        public IReadOnlyDictionary<GridItem, Vector2Int> ItemPositions => _itemPositions;

        private readonly Table<GridCell> _cells;
        private readonly Dictionary<GridItem, Vector2Int> _itemPositions = new();

        public Grid(int width, int height)
        {
            _cells = new Table<GridCell>(width, height);
            _itemPositions = new Dictionary<GridItem, Vector2Int>();
        }

        public bool ContainsItem(GridItem item) => _itemPositions.ContainsKey(item);

        public bool AddItem(GridItem item, Vector2Int position)
        {
            if (ContainsItem(item))
                throw new ArgumentException("Item already exists in grid");
            if (!CanAddItem(item, position))
                return false;
            AddItemUnsave(item, position);
            return true;
        }

        public IReadOnlyCollection<GridItem> RemoveItemsRecursive(GridItem targetItem)
        {
            if (!ContainsItem(targetItem))
                throw new ArgumentException("Item not exists in grid");
            var removeTargets = GetRecursiveRemoveTargets(targetItem);
            foreach (var item in removeTargets)
                RemoveItemUnsave(item);
            return removeTargets;
        }

        private void AddItemUnsave(GridItem item, Vector2Int position)
        {
            _itemPositions.Add(item, position);
            foreach (var cell in item.Cells)
            {
                var gridCell = _cells[cell.Position.x, cell.Position.y];
                gridCell.Add(item, cell);
            }
        }

        private void RemoveItemUnsave(GridItem item)
        {
            _itemPositions.Remove(item);
            foreach (var cell in item.Cells)
            {
                var gridCell = _cells[cell.Position.x, cell.Position.y];
                gridCell.Remove(item, cell);
            }
        }
    }
}
