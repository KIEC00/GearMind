namespace Assets.GearMind.State
{
    public interface IHaveState<TState> : IHaveState
        where TState : class
    {
        new TState GetState();
        void SetState(TState state);

        object IHaveState.GetState() => GetState();
        void IHaveState.SetState(object state) => SetState((TState)state);
    }
} 
