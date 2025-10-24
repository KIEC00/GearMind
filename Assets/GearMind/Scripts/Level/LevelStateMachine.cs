using System.Collections.Generic;

namespace Assets.GearMind.Level
{
    public class LevelStateMachine : ILevelStateMachine
    {
        public LevelState CurrentState { get; private set; } = LevelState.None;

        private readonly Dictionary<LevelState, ILevelState> _states = new();

        private ILevelState _activeState = null;

        public LevelStateMachine RegisterState(LevelState state, ILevelState levelState)
        {
            _states.Add(state, levelState);
            return this;
        }

        public bool CanTransitionTo(LevelState state) =>
            state != CurrentState && _states.ContainsKey(state);

        public bool TransitionTo(LevelState state)
        {
            if (!CanTransitionTo(state))
                return false;
            _activeState?.Exit();
            CurrentState = state;
            _activeState = _states[state];
            _activeState.Enter();
            return true;
        }
    }
}
