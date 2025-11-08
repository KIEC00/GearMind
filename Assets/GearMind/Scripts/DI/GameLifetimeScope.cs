using Assets.GearMind.Storage;
using Assets.GearMind.Storage.Endpoints;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<PrefsStorage>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<JsonSerializer>(Lifetime.Scoped).AsImplementedInterfaces();

            builder.RegisterEndpoint<LevelProgressEndpoint, string>("LevelProgress");
        }
    }
}
