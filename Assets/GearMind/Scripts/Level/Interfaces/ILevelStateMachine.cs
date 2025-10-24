namespace Assets.GearMind.Level
{
    public interface ILevelStateMachine
    {
        LevelState CurrentState { get; }
        bool CanTransitionTo(LevelState state);
        bool TransitionTo(LevelState state);
    }

    public enum LevelState
    {
        None,
        Edit,
        Simulate,
    }
}
