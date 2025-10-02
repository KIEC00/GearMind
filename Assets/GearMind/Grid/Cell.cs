using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public class Cell
    {
        [field: SerializeField]
        public Vector2Int Position { get; }

        [field: SerializeField]
        public CellFlags Flags { get; }

        public bool IsSolid => Flags.HasFlag(CellFlags.Solid);

        public CellFlags ProvideAttachableMask => Flags & CellFlags.AttachableAll;

        public CellFlags RequireAttachableMask =>
            (CellFlags)((short)(Flags & CellFlags.RequireAttachAll) >> 4);

        public Cell(Vector2Int position, CellFlags flags)
        {
            Position = position;
            Flags = flags;
        }

        public static CellFlags CombineFlags(IEnumerable<CellFlags> flags) =>
            flags.Aggregate(CellFlags.None, (acc, flag) => acc | flag);
    }
}
