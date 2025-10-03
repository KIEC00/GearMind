using Assets.GearMind.Scripts.UI;
using Assets.GearMind.Scripts.UI.Game;
using Assets.GearMind.Scripts.UI.Screens;
using R3;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.DI
{
    public class GameLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<Subject<Unit>>(Lifetime.Singleton);


            builder.Register<UIRootView>(Lifetime.Singleton).AsSelf();
            builder.Register<UIGameplayRootViewModel>(Lifetime.Singleton).AsSelf();

            builder.Register<GameplayUIManager>(Lifetime.Singleton).AsSelf();

            builder.Register<PauseMenuBinder>(Lifetime.Transient).AsSelf();
        }
    }
}