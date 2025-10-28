namespace Assets.GearMind.State
{
    public interface IStateService
    {
        void LoadStates();
        void Register(IHaveState entity, bool saveState = true);
        void SaveStates();
        void Unregister(IHaveState entity, bool loadState = false);
    }
}
