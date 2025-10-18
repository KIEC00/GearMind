using Unity.VisualScripting;
using UnityEngine;

namespace Assets.GearMind.Grid
{
    public readonly struct Cell
    {
        public Vector2Int Offset { get; }
        public CellType Type { get; }
        public CellFlags Flags { get; }

        public Cell(Vector2Int offset, CellType type, CellFlags flags)
        {
            Offset = offset;
            Type = type;
            Flags = flags;
        }

        public bool IsSolid => Type == CellType.Solid;
    }
}
