using Assets.GearMind.Audio;
using Assets.GearMind.Level;
using Assets.GearMind.Scripts.Level;
using Assets.GearMind.Storage;
using Assets.GearMind.Storage.Endpoints;
using EditorAttributes;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Dependencies")]
        [SerializeField, Required]
        private LevelProviderSO _levelProvider;

        [SerializeField, Required]
        private AudioVolumeControlComponent _audioVolumeControlComponent;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelProvider).AsImplementedInterfaces();
            builder.Register<LevelContextProvider>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PrefsStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<JsonSerializer>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterEndpoint<LevelProgressEndpoint, string>("LevelProgress");

            builder.RegisterInstance(_audioVolumeControlComponent).AsImplementedInterfaces();
        }
    }
}
