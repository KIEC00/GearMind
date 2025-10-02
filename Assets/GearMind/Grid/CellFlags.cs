using System;

namespace Assets.GearMind.Grid
{
    [Flags]
    public enum CellFlags : short
    {
        None = 0,

        Solid = 1 << 0,

        AttachableTop = 1 << 4,
        AttachableRight = 1 << 5,
        AttachableBottom = 1 << 6,
        AttachableLeft = 1 << 7,

        AttachableAll = AttachableTop | AttachableRight | AttachableBottom | AttachableLeft,

        RequireAttachTop = 1 << 8,
        RequireAttachRight = 1 << 9,
        RequireAttachBottom = 1 << 10,
        RequireAttachLeft = 1 << 11,

        RequireAttachAll = AttachableTop | AttachableRight | AttachableBottom | AttachableLeft,
    }
}
