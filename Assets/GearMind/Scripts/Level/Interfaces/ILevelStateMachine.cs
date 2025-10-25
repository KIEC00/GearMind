namespace Assets.GearMind.Level
{
    public interface ILevelStateMachine
    {
        LevelState CurrentState { get; }
        bool CanTransitionTo(LevelState state);
        bool TransitionTo(LevelState state);
        bool IsStatePaused { get; }
        void Continue();
        void Pause();
    }

    public enum LevelState
    {
        None,
        Edit,
        Simulate,
    }
}
