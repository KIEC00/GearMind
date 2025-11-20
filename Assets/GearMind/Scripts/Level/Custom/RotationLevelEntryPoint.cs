using System;
using Assets.GearMind.Custom.Input;
using Assets.GearMind.Level;
using Assets.GearMind.Scripts.UI;
using Assets.GearMind.Storage.Endpoints;
using Assets.GearMind.UI;
using GearMind.Services.Level;
using UnityEngine;
using VContainer.Unity;

namespace Assets.GearMind.Custom.Level
{
    public class RotationLevelEntryPoint : IStartable, IPostInitializable, IDisposable
    {
        private readonly Transform _enviromentAnchor;
        private readonly IRotationTarget _rotationTarget;
        private readonly IRotationInputService _inputService;
        private readonly ILevelGoalTrigger _levelGoalTrigger;
        private readonly IPauseService _pauseService;
        private readonly LevelContext _levelContext;
        private readonly LevelProgressEndpoint _levelProgressEndpoint;
        private readonly InterfaceContoller _interfaceContoller;

        public RotationLevelEntryPoint(
            Transform enviromentAnchor,
            IRotationTarget rotationTarget,
            IRotationInputService inputService,
            ILevelGoalTrigger levelGoalTrigger,
            IPauseService pauseService,
            LevelContext levelContext,
            LevelProgressEndpoint levelProgressEndpoint,
            InterfaceContoller interfaceContoller
        )
        {
            _enviromentAnchor = enviromentAnchor;
            _rotationTarget = rotationTarget;
            _inputService = inputService;
            _levelGoalTrigger = levelGoalTrigger;
            _pauseService = pauseService;
            _levelContext = levelContext;
            _levelProgressEndpoint = levelProgressEndpoint;
            _interfaceContoller = interfaceContoller;
        }

        public void PostInitialize() { }

        public void Start()
        {
            Subscribe();
            _inputService.Enable();
        }

        private void Subscribe()
        {
            _levelGoalTrigger.Trigger += OnLevelPassed;
            _pauseService.OnPauseChange += OnPauseChange;

            _inputService.RotationStart += _rotationTarget.StartRotation;
            _inputService.RotationStop += _rotationTarget.StopRotation;

            _interfaceContoller.OnRotateButtonStarted += _rotationTarget.StartRotation;
            _interfaceContoller.OnRotateButtonStoped += _rotationTarget.StopRotation;
        }

        private void Unsubscribe()
        {
            _levelGoalTrigger.Trigger -= OnLevelPassed;
            _pauseService.OnPauseChange -= OnPauseChange;

            _inputService.RotationStart -= _rotationTarget.StartRotation;
            _inputService.RotationStop -= _rotationTarget.StopRotation;

            _interfaceContoller.OnRotateButtonStarted -= _rotationTarget.StartRotation;
            _interfaceContoller.OnRotateButtonStoped -= _rotationTarget.StopRotation;
        }

        private void OnLevelPassed()
        {
            var passedLevelsIds = _levelProgressEndpoint.Load();
            if (!passedLevelsIds.Contains(_levelContext.Level.SceneID))
            {
                passedLevelsIds.Add(_levelContext.Level.SceneID);
                _levelProgressEndpoint.Save(passedLevelsIds);
            }
            _interfaceContoller.OnLevelPassed();
        }

        private void OnPauseChange(bool isPaused) => Time.timeScale = isPaused ? 0 : 1;

        public void Dispose()
        {
            Unsubscribe();
            _inputService.Disable();
        }
    }
}
