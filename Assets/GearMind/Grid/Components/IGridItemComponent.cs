using System.Collections.Generic;

namespace Assets.GearMind.Grid.Components
{
    public interface IGridItemComponent
    {
        public IEnumerable<Cell> Cells { get; }
    }

    public interface IRotatable
    {
        public void Rotate(RotationDirection direction = RotationDirection.Clockwise);
    }

    public enum RotationDirection : sbyte
    {
        Clockwise = -1,
        Counterclockwise = 1,
    }
}
