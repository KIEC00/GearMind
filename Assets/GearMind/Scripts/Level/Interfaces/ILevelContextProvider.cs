namespace Assets.GearMind.Level
{
    public interface ILevelContextProvider
    {
        LevelContext Current { get; }
    }
}
