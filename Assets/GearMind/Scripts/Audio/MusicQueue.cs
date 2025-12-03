using System.Collections.Generic;
using Assets.GearMind.Audio;
using UnityEngine;

[CreateAssetMenu(fileName = "MusicQueue", menuName = "GearMind/Music queue")]
public class MusicQueue : ScriptableObject, IMusicQueue
{
    public IEnumerable<AudioClip> ClipsCycle => GetClipsCycle();
    public int ClipsCount => _clips.Length;

    [SerializeField]
    private AudioClip[] _clips;

    private IEnumerable<AudioClip> GetClipsCycle()
    {
        while (true)
        {
            foreach (var clip in _clips)
                if (clip != null)
                    yield return clip;
        }
    }
}
