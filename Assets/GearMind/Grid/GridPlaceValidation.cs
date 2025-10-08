using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public partial class Grid
    {
        public bool IsPositionInBounds(Vector2Int position) =>
            position.x >= 0
            && position.x < _table.Size.width
            && position.y >= 0
            && position.y < _table.Size.height;

        public bool CanAddItem(GridItem item, Vector2Int position)
        {
            foreach (var cell in item.Cells)
            {
                var cellPosition = position + cell.Position;
                if (!IsPositionInBounds(cellPosition))
                    return false;
                if (!CanAddCell(cell, cellPosition))
                    return false;
            }
            return true;
        }

        public bool CanAddItem(
            GridItem item,
            Vector2Int position,
            out IEnumerable<GridItem> attachedItems
        )
        {
            attachedItems = null;
            if (!CanAddItem(item, position))
                return false;
            attachedItems = item.Cells.SelectMany(cell =>
                GetCellAttachedTo(cell.Flags, this[position + cell.Position])
            );
            return true;
        }

        public bool CanAddCell(Cell cell, Vector2Int cellPosition)
        {
            var gridCell = this[cellPosition.x, cellPosition.y];

            var flags = cell.Flags;
            var combinedFlags = gridCell.CombineFlags();

            if (flags.IsSolid() && combinedFlags.IsSolid())
                return false;

            var requireAttachMask = flags.GetRequireAttachMask();
            if (requireAttachMask == 0)
                return true;

            var combinedAttachableMask = combinedFlags.GetAttachableMask();
            if ((requireAttachMask & combinedAttachableMask) == CellFlags.None)
                return false;

            return true;
        }

        public IEnumerable<GridItem> GetCellAttachedTo(CellFlags flags, IReadOnlyGridCell gridCell)
        {
            var requireAttachMask = flags.GetRequireAttachMask();
            if (requireAttachMask == 0)
                yield break;
            foreach (var record in gridCell)
            {
                var attachableMask = record.Cell.Flags.GetAttachableMask();
                if ((requireAttachMask & attachableMask) != CellFlags.None)
                    yield return record.Item;
            }
        }

        private IReadOnlyCollection<GridItem> GetRecursiveRemoveTargets(GridItem itemToRemove)
        {
            var stack = new Stack<GridItem>();
            var targets = new HashSet<GridItem>();

            stack.Push(itemToRemove);
            targets.Add(itemToRemove);

            while (stack.Count > 0)
            {
                var current = stack.Pop();
                foreach (var cell in current.Cells)
                {
                    var cellPosition = current.Position + cell.Position;
                    var gridCell = _table[cellPosition.x, cellPosition.y];

                    var combinedFlags = Cell.CombineFlags(
                        gridCell
                            .Where(record => !targets.Contains(record.Item))
                            .Select(record => record.Cell.Flags)
                    );

                    var selfProvideAttachMask = cell.Flags.GetAttachableMask();
                    var otherProvideAttachMask = combinedFlags.GetAttachableMask();
                    var otherRequireAttachFlags = combinedFlags.GetRequireAttachMask();

                    var onlySelfProvideAttachMask =
                        selfProvideAttachMask & ~otherProvideAttachMask & otherRequireAttachFlags;

                    if (onlySelfProvideAttachMask == CellFlags.None)
                        continue;

                    foreach (var record in gridCell)
                    {
                        var item = record.Item;
                        if (targets.Contains(item))
                            continue;
                        var requireAttachMask = record.Cell.Flags.GetRequireAttachMask();
                        if ((requireAttachMask & onlySelfProvideAttachMask) == CellFlags.None)
                            continue;
                        stack.Push(item);
                        targets.Add(item);
                    }
                }
            }

            return targets;
        }
    }
}
