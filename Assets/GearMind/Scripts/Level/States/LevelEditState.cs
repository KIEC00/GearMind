using Assets.GearMind.State;
using Assets.GearMind.UI;

namespace Assets.GearMind.Level
{
    public class LevelEditState : ILevelState
    {
        private readonly PlacementService _placementService;
        private readonly IGameplayObjectService _objectService;
        private readonly IStateService _stateService;
        private readonly UIManager _uiManager;

        public LevelEditState(
            PlacementService placementService,
            IGameplayObjectService objectService,
            IStateService stateService,
            UIManager uiManager
        )
        {
            _placementService = placementService;
            _objectService = objectService;
            _stateService = stateService;
            _uiManager = uiManager;
        }

        public void Enter()
        {
            _placementService.Enable();
            _objectService.EnterEditMode();
            _stateService.LoadStates();
            _uiManager.EnterEditMode();
        }

        public void Exit()
        {
            _placementService.Disable();
            _stateService.SaveStates();
        }

        public void Continue()
        {
            _placementService.Enable();
        }

        public void Pause()
        {
            _placementService.Disable();
        }
    }
}
