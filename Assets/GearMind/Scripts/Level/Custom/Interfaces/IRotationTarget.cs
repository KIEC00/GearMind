using Assets.GearMind.Custom.Input;

namespace Assets.GearMind.Custom.Level
{
    public interface IRotationTarget
    {
        void StartRotation(Direction direction);
        void StopRotation();
    }
}
