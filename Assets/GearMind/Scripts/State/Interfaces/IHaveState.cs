namespace Assets.GearMind.State
{
    public interface IHaveState
    {
        object GetState();
        void SetState(object state);
    }
}
