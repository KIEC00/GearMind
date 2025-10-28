using System.Collections.Generic;

namespace Assets.GearMind.Level
{
    public class LevelStateMachine : ILevelStateMachine
    {
        public LevelState CurrentState { get; private set; } = LevelState.None;
        public bool IsStatePaused => _paused;

        private readonly Dictionary<LevelState, ILevelState> _states = new();
        private ILevelState _activeState = null;
        private bool _paused = false;

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
            _activeState = _states.GetValueOrDefault(state, null);
            _paused = false;
            _activeState?.Enter();
            return true;
        }

        public void Continue()
        {
            if (!_paused)
                return;
            _activeState?.Continue();
            _paused = false;
        }

        public void Pause()
        {
            if (_paused)
                return;
            _activeState?.Pause();
            _paused = true;
        }
    }
}
