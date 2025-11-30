namespace Assets.GearMind.Audio
{
    public interface IAudioVolumeControl
    {
        float this[AudioChannel channel] { get; set; }
    }
}
