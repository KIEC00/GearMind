using R3;
using System;

namespace Assets.GearMind.Scripts.UI
{
    public abstract class WindowViewModel : IDisposable
    {
        public Observable<WindowViewModel> CloseRequested => _closeRequested;
        public abstract string Id { get; } // Для поиска префабов UI

        private readonly Subject<WindowViewModel> _closeRequested = new();

        public void RequestClose()
        {
            _closeRequested.OnNext(this);
        }

        public virtual void Dispose() { }
    }
}