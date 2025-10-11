using System;

namespace Assets.GearMind.Grid
{
    [Flags]
    public enum CellFlags : byte
    {
        None = 0,

        AttachTop = 1 << 0,
        AttachRight = 1 << 1,
        AttachBottom = 1 << 2,
        AttachLeft = 1 << 3,

        AttachAllSides = AttachTop | AttachRight | AttachBottom | AttachLeft,
    }
}
