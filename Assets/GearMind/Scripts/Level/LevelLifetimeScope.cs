using System.Collections.Generic;
using System.Linq;
using Assets.GearMind.Common;
using Assets.GearMind.Grid;
using Assets.GearMind.Input;
using Assets.GearMind.Inventory;
using Assets.GearMind.Level.States;
using Assets.GearMind.State;
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
        private Transform _environmentAnchor;

        [SerializeField, Required]
        private GraphicRaycaster _graphicRaycaster;

        [SerializeField, Required]
        private EventSystem _eventSystem;

        [SerializeField, Required]
        private GridComponent _grid;

        protected override void Configure(IContainerBuilder builder)
        {
            builder
                .RegisterEntryPoint<LevelEntryPoint>(Lifetime.Singleton)
                .WithParameter(_environmentAnchor);

            builder.RegisterInstance(_inventoryFactory.CreateInventory()).As<IInventory>();
            builder.RegisterInstance(CreateIdentityPrefabMap(_inventoryFactory));

            builder.RegisterInstance(_graphicRaycaster);
            builder.RegisterInstance(_eventSystem);
            builder.RegisterInstance(_grid);

            builder.Register<PauseService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Transient).AsImplementedInterfaces();
            builder.Register<CameraProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ScreenRaycaster>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.Register<LevelEditState>(Lifetime.Singleton).AsSelf();
            builder.Register<LevelSimulationState>(Lifetime.Singleton).AsSelf();

            builder.Register<ObjectService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<StateService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameplayObjectService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder
                .Register<PlacementService>(Lifetime.Singleton)
                .WithParameter("errorDragZOffset", -1f)
                .All();

            builder.Register(LevelStateMachineFactoryMethod, Lifetime.Singleton).All();
        }

        private static LevelStateMachine LevelStateMachineFactoryMethod(IObjectResolver c) =>
            new LevelStateMachine()
                .RegisterState(LevelState.Edit, c.Resolve<LevelEditState>())
                .RegisterState(LevelState.Simulate, c.Resolve<LevelSimulationState>());

        private static IIdentityPrefabMap CreateIdentityPrefabMap(
            InventoryFactorySO inventoryFactory
        ) =>
            new IdentityPrefabMap(
                inventoryFactory
                    .GetComponents()
                    .Select(component => new KeyValuePair<IInventoryIdentity, GameObject>(
                        component.InventoryIdentity,
                        component.gameObject
                    ))
            );
    }
}
