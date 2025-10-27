namespace Assets.GearMind.Level
{
    public class LevelEditState : ILevelState
    {
        private readonly PlacementService _placementService;
        private readonly IObjectService _objectService;

        public LevelEditState(PlacementService placementService, IObjectService objectService)
        {
            _placementService = placementService;
            _objectService = objectService;
        }

        public void Enter()
        {
            _placementService.Enable();
            _objectService.EnterEditMode();
        }

        public void Exit()
        {
            _placementService.Disable();
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
