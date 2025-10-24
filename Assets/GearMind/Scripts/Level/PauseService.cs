using System;
using GearMind.Services.Level;

public class PauseService : IPauseService, IDisposable
{
    public bool IsPaused { get; private set; }

    public event Action<bool> OnPauseChange;

    public void Pause()
    {
        if (IsPaused)
            return;
        IsPaused = true;
        OnPauseChange?.Invoke(IsPaused);
    }

    public void Unpause()
    {
        if (!IsPaused)
            return;
        IsPaused = false;
        OnPauseChange?.Invoke(IsPaused);
    }

    public void TogglePause()
    {
        if (IsPaused)
            Unpause();
        else
            Pause();
    }

    public void Dispose() => Unpause();
}
