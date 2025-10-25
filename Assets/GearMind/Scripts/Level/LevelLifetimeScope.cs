using Assets.GearMind.Common;
using Assets.GearMind.Input;
using Assets.GearMind.Level.States;
using Assets.Utils.Runtime;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelEntryPoint>(Lifetime.Singleton);

            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<CameraProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScreenRaycaster>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LevelEditState>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelSimulationState>(Lifetime.Singleton).AsSelf();

            builder.Register(LevelStateMachineFactoryMethod, Lifetime.Singleton).All();
        }

        private static LevelStateMachine LevelStateMachineFactoryMethod(IObjectResolver c) =>
            new LevelStateMachine()
                .RegisterState(LevelState.Edit, c.Resolve<LevelEditState>())
                .RegisterState(LevelState.Simulate, c.Resolve<LevelSimulationState>());
    }
}
