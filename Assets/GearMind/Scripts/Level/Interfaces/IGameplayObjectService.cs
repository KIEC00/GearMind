using Assets.GearMind.Instruments;

namespace Assets.GearMind.Level
{
    public interface IGameplayObjectService
    {
        void Register(IGameplayObject gameplayObject);
        void Unregister(IGameplayObject gameplayObject);
        void EnterEditMode();
        void EnterSimulationMode();
    }
}
