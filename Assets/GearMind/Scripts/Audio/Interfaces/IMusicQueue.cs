using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Audio
{
    public interface IMusicQueue
    {
        public IEnumerable<AudioClip> ClipsCycle { get; }
        public int ClipsCount { get; }
    }
}
