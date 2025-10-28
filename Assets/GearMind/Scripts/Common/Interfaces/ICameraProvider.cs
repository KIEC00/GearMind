using UnityEngine;

namespace Assets.GearMind.Common
{
    public interface ICameraProvider
    {
        Camera Current { get; }
    }
}
