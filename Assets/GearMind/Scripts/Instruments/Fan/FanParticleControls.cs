using EditorAttributes;
using UnityEngine;

public class FanParticleControls : MonoBehaviour
{
    [SerializeField, Required]
    private ParticleSystem _windParticles;

    [SerializeField, Required]
    private ParticleSystem _leafParticles;

    public void StartEmmiting()
    {
        _windParticles.Play(withChildren: false);
        _leafParticles.Play(withChildren: false);
    }

    public void StopEmmiting()
    {
        _windParticles.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
        _leafParticles.Stop(withChildren: false, ParticleSystemStopBehavior.StopEmittingAndClear);
    }

    public void SetSize(float size)
    {
        var windMain = _windParticles.main;
        windMain.startLifetime = 0.5f * size;
        var leafMain = _leafParticles.main;
        leafMain.startLifetime = 0.5f * size;
    }
}
