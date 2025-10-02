using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public partial class Grid
    {
        public bool IsPositionInBounds(Vector2Int position) =>
            position.x >= 0
            && position.x < _cells.Size.width
            && position.y >= 0
            && position.y < _cells.Size.height;

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

        public bool CanAddCell(Cell cell, Vector2Int cellPosition)
        {
            var gridCell = _cells[cellPosition.x, cellPosition.y];

            var flags = cell.Flags;
            var combinedFlags = CombineFlags(gridCell);

            if (flags.HasFlag(CellFlags.Solid) && combinedFlags.HasFlag(CellFlags.Solid))
                return false;

            var requireAttachFlags = flags & CellFlags.RequireAttachAll;
            if (requireAttachFlags == CellFlags.None)
                return true;

            var attachMask = (CellFlags)((int)requireAttachFlags << 4);

            var combinedAttachableFlags = combinedFlags & CellFlags.AttachableAll;
            if ((attachMask & combinedAttachableFlags) == CellFlags.None)
                return false;

            return true;
        }

        public IReadOnlyCollection<GridItem> GetRecursiveRemoveTargets(GridItem itemToRemove)
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
                    var cellPosition = _itemPositions[current] + cell.Position;
                    var gridCell = _cells[cellPosition.x, cellPosition.y];

                    var attachableFlags = cell.Flags & CellFlags.AttachableAll;
                    if (attachableFlags == CellFlags.None)
                        continue;

                    var requireAttachMask = (CellFlags)((int)attachableFlags >> 4);

                    var combinedFlags = CombineFlags(
                        gridCell
                            .Records.Where(record => !targets.Contains(record.Item))
                            .Select(record => record.Cell.Flags)
                    );

                    var provideAttachMask = requireAttachMask & combinedFlags;
                    var provideAttachOtherFlags = attachableFlags & combinedFlags;
                    var provideAttachOtherMask = (CellFlags)((int)provideAttachOtherFlags >> 4);
                    var selfProvideAttachMask = provideAttachMask & provideAttachOtherMask;

                    if (selfProvideAttachMask != 0)
                        continue;

                    foreach (var record in gridCell.Records)
                    {
                        if (targets.Contains(record.Item))
                            continue;
                        if ((record.Cell.Flags & selfProvideAttachMask) != 0)
                            stack.Push(record.Item);
                    }
                }
            }

            return targets;
        }

        public CellFlags CombineFlags(IReadOnlyGridCell gridCell) =>
            CombineFlags(gridCell.Records.Select(record => record.Cell.Flags));

        public CellFlags CombineFlags(IEnumerable<CellFlags> flags) =>
            flags.Aggregate(CellFlags.None, (acc, flag) => acc | flag);
    }
}
