namespace Assets.GearMind.Objects
{
    interface IGameplayObject
    {
        void EnterEditMode();
        void SaveState();
        void EnterPlayMode();
        void LoadState();
    }
}
