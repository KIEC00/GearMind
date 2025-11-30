using System.Collections.Generic;
using Assets.GearMind.Audio;

public static class AudioMixerKeys
{
    public static IReadOnlyDictionary<AudioChannel, string> Volume => _volume;
    public const string MusicVolumeKey = "MusicVolume";
    public const string SFXVolumeKey = "SFXVolume";

    private static readonly Dictionary<AudioChannel, string> _volume =
        new() { [AudioChannel.Music] = MusicVolumeKey, [AudioChannel.SFX] = SFXVolumeKey };
}
