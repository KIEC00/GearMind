using System;
using Assets.GearMind.Level;
using VContainer.Unity;

public class LevelEntryPoint : IPostStartable, IDisposable
{
    private LevelStateMachine _levelStateMachine;

    public LevelEntryPoint(LevelStateMachine levelStateMachine)
    {
        _levelStateMachine = levelStateMachine;
    }

    public void PostStart() => _levelStateMachine.Mode = LevelMode.Edit;

    public void Dispose() { }
}
