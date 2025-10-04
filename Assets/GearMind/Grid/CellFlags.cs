using System;

namespace Assets.GearMind.Grid
{
    [Flags]
    public enum CellFlags : short
    {
        None = 0,

        AttachableTop = 1 << 0,
        AttachableRight = 1 << 1,
        AttachableBottom = 1 << 2,
        AttachableLeft = 1 << 3,

        AttachableAll = AttachableTop | AttachableRight | AttachableBottom | AttachableLeft,

        RequireAttachTop = 1 << 4,
        RequireAttachRight = 1 << 5,
        RequireAttachBottom = 1 << 6,
        RequireAttachLeft = 1 << 7,

        RequireAttachAll =
            RequireAttachTop | RequireAttachRight | RequireAttachBottom | RequireAttachLeft,

        Solid = 1 << 14,
    }

    public static class CellFlagsExtensions
    {
        public static bool IsSolid(this CellFlags cellFlags) => (cellFlags & CellFlags.Solid) != 0;

        public static CellFlags GetAttachableMask(this CellFlags flags) =>
            flags & CellFlags.AttachableAll;

        public static CellFlags GetRequireAttachMask(this CellFlags flags) =>
            (CellFlags)((short)(flags & CellFlags.RequireAttachTop) >> 4);
    }
}
