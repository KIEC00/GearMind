using System;
using Assets.GearMind.Instruments;
using Assets.GearMind.State;
using Assets.GearMind.Storage.Endpoints;
using Assets.GearMind.UI;
using GearMind.Services.Level;
using UnityEngine;
using VContainer.Unity;

namespace Assets.GearMind.Level
{
    public class LevelEntryPoint : IStartable, IPostInitializable, IDisposable
    {
        private readonly LevelStateMachine _levelStateMachine;
        private readonly IGameplayObjectService _gameplayObjectService;
        private readonly IStateService _stateService;
        private readonly Transform _anchor;
        private readonly UIManager _uiManager;
        private readonly ILevelGoalTrigger _levelGoalTrigger;
        private readonly IPauseService _pauseService;
        private readonly LevelContext _levelContext;
        private readonly LevelProgressEndpoint _levelProgressEndpoint;

        public LevelEntryPoint(
            Transform anchor,
            LevelStateMachine levelStateMachine,
            IGameplayObjectService gameplayObjectService,
            IStateService stateService,
            UIManager uiManager,
            ILevelGoalTrigger levelGoalTrigger,
            IPauseService pauseService,
            LevelProgressEndpoint levelProgressEndpoint,
            LevelContext levelContext
        )
        {
            _anchor = anchor;
            _levelStateMachine = levelStateMachine;
            _gameplayObjectService = gameplayObjectService;
            _stateService = stateService;
            _uiManager = uiManager;
            _levelGoalTrigger = levelGoalTrigger;
            _pauseService = pauseService;
            _levelContext = levelContext;
            _levelProgressEndpoint = levelProgressEndpoint;
        }

        public void PostInitialize()
        {
            var gpo = _anchor.GetComponentsInChildren<IGameplayObject>(includeInactive: true);
            foreach (var obj in gpo)
                _gameplayObjectService.Register(obj);
            var sto = _anchor.GetComponentsInChildren<IHaveState>(includeInactive: true);
            foreach (var obj in sto)
                _stateService.Register(obj);
            _uiManager.OnModeChanged += OnModeChanged;
            _levelGoalTrigger.Trigger += OnLevelPassed;
            _pauseService.OnPauseChange += OnPauseChange;
        }

        private void OnModeChanged(bool IsEditMode) =>
            _levelStateMachine.TransitionTo(IsEditMode ? LevelState.Edit : LevelState.Simulate);

        public void Start() => _levelStateMachine.TransitionTo(LevelState.Edit);

        private void OnLevelPassed()
        {
            var passedLevelsIds = _levelProgressEndpoint.Load();
            if (!passedLevelsIds.Contains(_levelContext.Level.SceneID))
            {
                passedLevelsIds.Add(_levelContext.Level.SceneID);
                _levelProgressEndpoint.Save(passedLevelsIds);
            }
            _uiManager.OpenNextLevelMenu();
        }

        private void OnPauseChange(bool isPaused)
        {
            if (isPaused)
                _levelStateMachine.Pause();
            else
                _levelStateMachine.Continue();
        }

        public void Dispose()
        {
            _pauseService.Unpause();
            _uiManager.OnModeChanged -= OnModeChanged;
            _levelGoalTrigger.Trigger -= OnLevelPassed;
            _pauseService.OnPauseChange -= OnPauseChange;
        }
    }
}
