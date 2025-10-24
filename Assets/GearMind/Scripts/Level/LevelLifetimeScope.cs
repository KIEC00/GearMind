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

            builder.Register<LevelEditState>(Lifetime.Singleton);
            builder.Register<LevelSimulationState>(Lifetime.Singleton);

            builder.Register(LevelStateMachineFactoryMethod, Lifetime.Singleton).BindAll();
        }

        private static LevelStateMachine LevelStateMachineFactoryMethod(IObjectResolver c) =>
            new LevelStateMachine()
                .RegisterState(LevelState.Edit, c.Resolve<LevelEditState>())
                .RegisterState(LevelState.Simulate, c.Resolve<LevelSimulationState>());
    }
}
