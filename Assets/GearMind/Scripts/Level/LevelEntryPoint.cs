using Assets.GearMind.Objects;
using Assets.GearMind.State;
using UnityEngine;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelEntryPoint : IStartable, IPostInitializable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly IObjectService _objectService;
        private readonly IStateService _stateService;
        private readonly Transform _anchor;

        public LevelEntryPoint(
            Transform anchor,
            LevelStateMachine levelStateMachine,
            IObjectService objectService,
            IStateService stateService
        )
        {
            _anchor = anchor;
            _levelStateMachine = levelStateMachine;
            _objectService = objectService;
            _stateService = stateService;
        }

        public void PostInitialize()
        {
            var gpo = _anchor.GetComponentsInChildren<IGameplayObject>(includeInactive: true);
            foreach (var obj in gpo)
                _objectService.RegisterGameplayObject(obj);
            var sto = _anchor.GetComponentsInChildren<IHaveState>(includeInactive: true);
            foreach (var obj in sto)
                _stateService.Register(obj);
        }

        public void Start()
        {
            _levelStateMachine.TransitionTo(LevelState.Edit);
        }
    }
}
