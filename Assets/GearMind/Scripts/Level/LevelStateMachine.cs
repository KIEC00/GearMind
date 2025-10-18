using System;
using System.Collections.Generic;
using System.Linq;

namespace Assets.GearMind.Level
{
    public class LevelStateMachine : IDisposable
    {
        public LevelMode Mode
        {
            get => _mode;
            set => HandleSwitchMode(_mode, value);
        }

        public bool Paused
        {
            get => _paused;
            set => HandlePause(value);
        }

        private LevelMode _mode;
        private bool _paused;
        private Action<bool> _pauseCallback;
        private readonly Dictionary<LevelMode, ModeCallbacks> _modeCallbacks;

        public LevelStateMachine()
        {
            _mode = LevelMode.Init;
            _paused = false;
            _modeCallbacks = Enum.GetValues(typeof(LevelMode))
                .Cast<LevelMode>()
                .ToDictionary(mode => mode, mode => new ModeCallbacks());
        }

        public void SubscribePause(Action<bool> callback)
        {
            _pauseCallback -= callback;
            _pauseCallback += callback;
        }

        public void UnsubscribePause(Action<bool> callback) => _pauseCallback -= callback;

        public void SubscribeModeEnter(LevelMode mode, Action callback)
        {
            _modeCallbacks[mode].Enter -= callback;
            _modeCallbacks[mode].Enter += callback;
        }

        public void SubscribeModeExit(LevelMode mode, Action callback)
        {
            _modeCallbacks[mode].Exit -= callback;
            _modeCallbacks[mode].Exit += callback;
        }

        public void UnsubscribeModeEnter(LevelMode mode, Action callback) =>
            _modeCallbacks[mode].Enter -= callback;

        public void UnsubscribeModeExit(LevelMode mode, Action callback) =>
            _modeCallbacks[mode].Exit -= callback;

        private void HandleSwitchMode(LevelMode from, LevelMode to)
        {
            if (from == to)
                return;
            if (from == LevelMode.Disposed || to == LevelMode.Disposed)
                throw new InvalidOperationException("Cannot switch from/to disposed mode");
            _mode = to;
            _modeCallbacks[from].Exit?.Invoke();
            _modeCallbacks[to].Enter?.Invoke();
        }

        private void HandlePause(bool value)
        {
            if (value == _paused)
                return;
            _paused = value;
            _pauseCallback?.Invoke(value);
        }

        public void Dispose()
        {
            _mode = LevelMode.Disposed;
            _modeCallbacks[LevelMode.Disposed].Enter?.Invoke();
            foreach (var callbacks in _modeCallbacks.Values)
            {
                callbacks.Enter = null;
                callbacks.Exit = null;
            }
        }

        private class ModeCallbacks
        {
            public Action Enter;
            public Action Exit;
        }
    }

    public enum LevelMode : byte
    {
        Init,
        Edit,
        Play,
        Disposed,
    }
}
