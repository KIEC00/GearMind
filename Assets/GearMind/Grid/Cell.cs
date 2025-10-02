using UnityEngine;

namespace Assets.GearMind.Grid
{
    public class Cell
    {
        [field: SerializeField]
        public Vector2Int Position { get; }

        [field: SerializeField]
        public CellFlags Flags { get; }

        public Cell(Vector2Int position, CellFlags flags)
        {
            Position = position;
            Flags = flags;
        }
    }
}
