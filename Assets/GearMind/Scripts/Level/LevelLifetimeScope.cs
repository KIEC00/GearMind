using Assets.GearMind.Scripts.UI;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LevelStateMachine>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelManager>(Lifetime.Singleton).AsSelf();

            builder.RegisterComponentInHierarchy<NextLevelController>();

            builder.RegisterEntryPoint<LevelEntryPoint>();
        }
    }
}
