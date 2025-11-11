using Assets.GearMind.Level;
using Assets.GearMind.Scripts.Level;
using Assets.GearMind.Storage;
using Assets.GearMind.Storage.Endpoints;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        [Header("Dependencies")]
        [SerializeField]
        private LevelProviderSO _levelProvider;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_levelProvider).AsImplementedInterfaces();
            builder.Register<LevelContextProvider>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<PrefsStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<JsonSerializer>(Lifetime.Scoped).AsImplementedInterfaces();
            builder.RegisterEndpoint<LevelProgressEndpoint, string>("LevelProgress");
        }
    }
}
