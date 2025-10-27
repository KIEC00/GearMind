using System.Linq;
using Assets.GearMind.Common;
using Assets.GearMind.Grid;
using Assets.GearMind.Input;
using Assets.GearMind.Inventory;
using Assets.GearMind.Level.States;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using VContainer;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelLifetimeScope : LifetimeScope
    {
        [Header("Scriptable Objects")]
        [SerializeField, Required]
        private InventoryFactorySO _inventoryFactory;

        [Header("Components")]
        [SerializeField, Required]
        private GraphicRaycaster _graphicRaycaster;

        [SerializeField, Required]
        private EventSystem _eventSystem;

        [SerializeField, Required]
        private GridComponent _grid;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<LevelEntryPoint>(Lifetime.Singleton);

            builder.RegisterInstance(_inventoryFactory.CreateInventory()).As<IInventory>();

            builder.RegisterInstance(_graphicRaycaster);
            builder.RegisterInstance(_eventSystem);
            builder.RegisterInstance(_grid);

            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Transient).AsImplementedInterfaces();
            builder.Register<CameraProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScreenRaycaster>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LevelEditState>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelSimulationState>(Lifetime.Singleton).AsSelf();

            builder.Register<PlacementService>(Lifetime.Singleton).All();

            builder.Register(LevelStateMachineFactoryMethod, Lifetime.Singleton).All();
        }

        private static LevelStateMachine LevelStateMachineFactoryMethod(IObjectResolver c) =>
            new LevelStateMachine()
                .RegisterState(LevelState.Edit, c.Resolve<LevelEditState>())
                .RegisterState(LevelState.Simulate, c.Resolve<LevelSimulationState>());
    }
}
