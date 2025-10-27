using Assets.GearMind.Objects;
using UnityEngine;

namespace Assets.GearMind.Level
{
    public interface IObjectService
    {
        void InstantiateObject(GameObject prefab);
        void DestroyObject(GameObject instance);
        void RegisterGameplayObject(IGameplayObject gameplayObject);
        void UnregisterGameplayObject(IGameplayObject gameplayObject);
        void EnterEditMode();
        void EnterSimulationMode();
        void Dispose();
    }
}
