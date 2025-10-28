using System;

namespace GearMind.Services.Level
{
    public interface IPauseService
    {
        bool IsPaused { get; }
        event Action<bool> OnPauseChange;
        void Pause();
        void Unpause();
        void TogglePause();
    }
}
