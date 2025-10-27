namespace Assets.GearMind.Level
{
    public class LevelEditState : ILevelState
    {
        private readonly PlacementService _placementService;

        public LevelEditState(PlacementService placementService)
        {
            _placementService = placementService;
        }

        public void Enter()
        {
            _placementService.Enable();
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
