using System.Collections.Generic;
using Assets.GearMind.Instruments;

namespace Assets.GearMind.Level
{
    public class GameplayObjectService : IGameplayObjectService
    {
        private readonly HashSet<IGameplayObject> _gameplayObjects = new();

        public void Register(IGameplayObject gameplayObject) =>
            _gameplayObjects.Add(gameplayObject);

        public void Unregister(IGameplayObject gameplayObject) =>
            _gameplayObjects.Remove(gameplayObject);

        public void EnterEditMode()
        {
            foreach (var go in _gameplayObjects)
                go.EnterEditMode();
        }

        public void EnterSimulationMode()
        {
            foreach (var go in _gameplayObjects)
                go.EnterPlayMode();
        }
    }
}
