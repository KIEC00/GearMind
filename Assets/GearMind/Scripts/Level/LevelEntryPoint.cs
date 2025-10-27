using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelEntryPoint : IStartable
    {
        private readonly LevelStateMachine _levelStateMachine;

        public LevelEntryPoint(LevelStateMachine levelStateMachine)
        {
            _levelStateMachine = levelStateMachine;
        }

        public void Start()
        {
            _levelStateMachine.TransitionTo(LevelState.Edit);
        }
    }
}
