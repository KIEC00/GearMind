using Assets.GearMind.Objects;
using UnityEngine;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelEntryPoint : IStartable, IPostInitializable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly IObjectService _objectService;
        private readonly Transform _anchor;

        public LevelEntryPoint(
            Transform anchor,
            LevelStateMachine levelStateMachine,
            IObjectService objectService
        )
        {
            _anchor = anchor;
            _levelStateMachine = levelStateMachine;
            _objectService = objectService;
        }

        public void PostInitialize()
        {
            var objects = _anchor.GetComponentsInChildren<IGameplayObject>(includeInactive: true);
            foreach (var obj in objects)
                _objectService.RegisterGameplayObject(obj);
        }

        public void Start()
        {
            _levelStateMachine.TransitionTo(LevelState.Edit);
        }
    }
}
