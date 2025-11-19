using System;
using Assets.GearMind.Common;

namespace Assets.GearMind.Custom.Input
{
    public interface IRotationInputService : IToggable
    {
        Direction? RotationDirection { get; }

        event Action<Direction> RotationStart;
        event Action RotationStop;
        event Action EscPressed;
    }

    public enum Direction : int
    {
        Clockwise = -1,
        Counterclockwise = 1,
    }
}
