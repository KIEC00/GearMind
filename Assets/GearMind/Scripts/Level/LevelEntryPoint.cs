using System;
using Assets.GearMind.Input;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelEntryPoint : IStartable, IDisposable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly IInputService _input;

        public LevelEntryPoint(LevelStateMachine levelStateMachine, IInputService input)
        {
            _levelStateMachine = levelStateMachine;
            _input = input;
        }

        public void Start()
        {
            _levelStateMachine.TransitionTo(LevelState.Edit);
            _input.Enable();
        }

        public void Dispose() => _input.Disable();
    }
}
