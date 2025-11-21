namespace Assets.GearMind.Common
{
    public interface IToggable
    {
        bool Enabled { get; set; }
        void Enable();
        void Disable();
    }
}
