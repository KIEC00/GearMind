namespace Assets.GearMind.Level
{
    public interface ILevelState
    {
        void Enter();
        void Pause();
        void Continue();
        void Exit();
    }
}
