using Assets.GearMind.State;

namespace Assets.GearMind.Level
{
    public class LevelEditState : ILevelState
    {
        private readonly PlacementService _placementService;
        private readonly IObjectService _objectService;
        private readonly IStateService _stateService;

        public LevelEditState(
            PlacementService placementService,
            IObjectService objectService,
            IStateService stateService
        )
        {
            _placementService = placementService;
            _objectService = objectService;
            _stateService = stateService;
        }

        public void Enter()
        {
            _placementService.Enable();
            _objectService.EnterEditMode();
            _stateService.LoadStates();
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
