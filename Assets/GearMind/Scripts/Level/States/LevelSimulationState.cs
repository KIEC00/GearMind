namespace Assets.GearMind.Level.States
{
    public class LevelSimulationState : ILevelState
    {
        private readonly IGameplayObjectService _gameplayObjectService;

        public LevelSimulationState(IGameplayObjectService gameplayObjectService)
        {
            _gameplayObjectService = gameplayObjectService;
        }

        public void Enter()
        {
            _gameplayObjectService.EnterSimulationMode();
        }

        public void Exit() { }

        public void Continue() { }

        public void Pause() { }
    }
}
