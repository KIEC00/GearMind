namespace Assets.GearMind.Level.States
{
    public class LevelSimulationState : ILevelState
    {
        private readonly IObjectService _objectService;

        public LevelSimulationState(IObjectService objectService)
        {
            _objectService = objectService;
        }

        public void Enter()
        {
            _objectService.EnterSimulationMode();
        }

        public void Exit() { }

        public void Continue() { }

        public void Pause() { }
    }
}
