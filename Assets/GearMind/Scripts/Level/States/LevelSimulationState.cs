using Assets.GearMind.UI;

namespace Assets.GearMind.Level.States
{
    public class LevelSimulationState : ILevelState
    {
        private readonly IGameplayObjectService _gameplayObjectService;
        private readonly UIManager _uiManager;

        public LevelSimulationState(
            IGameplayObjectService gameplayObjectService,
            UIManager uiManager
        )
        {
            _gameplayObjectService = gameplayObjectService;
            _uiManager = uiManager;
        }

        public void Enter()
        {
            _gameplayObjectService.EnterSimulationMode();
            _uiManager.EnterSimulateMode();
        }

        public void Exit() { }

        public void Continue() { }

        public void Pause() { }
    }
}
