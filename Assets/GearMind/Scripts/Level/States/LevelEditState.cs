using System.Collections;
using Assets.GearMind.Common;
using Assets.GearMind.State;
using Assets.GearMind.UI;
using UnityEditor;
using UnityEngine;

namespace Assets.GearMind.Level
{
    public class LevelEditState : ILevelState
    {
        private const float TARGET_FOV = 1f;
        private const float FOV_CHANGE_DURATION = 0.1f;

        private readonly PlacementService _placementService;
        private readonly IGameplayObjectService _objectService;
        private readonly IStateService _stateService;
        private readonly UIManager _uiManager;
        private readonly FOVControlComponent _fovControlComponent;

        private readonly float _initialFOVValue;
        private Coroutine _FOVCoroutine;

        public LevelEditState(
            PlacementService placementService,
            IGameplayObjectService objectService,
            IStateService stateService,
            UIManager uiManager,
            FOVControlComponent fovControlComponent
        )
        {
            _placementService = placementService;
            _objectService = objectService;
            _stateService = stateService;
            _uiManager = uiManager;
            _fovControlComponent = fovControlComponent;
            _initialFOVValue = _fovControlComponent.FOV;
        }

        public void Enter()
        {
            _placementService.Enable();
            _objectService.EnterEditMode();
            _stateService.LoadStates();
            _uiManager.EnterEditMode();
            SmoothSetFOV(TARGET_FOV, FOV_CHANGE_DURATION);
        }

        public void Exit()
        {
            _placementService.Disable();
            _stateService.SaveStates();
            SmoothSetFOV(_initialFOVValue, FOV_CHANGE_DURATION);
        }

        public void Continue()
        {
            _placementService.Enable();
        }

        public void Pause()
        {
            _placementService.Disable();
        }

        public Coroutine SmoothSetFOV(float targetFOV, float duration)
        {
            if (_FOVCoroutine != null)
                _fovControlComponent.StopCoroutine(_FOVCoroutine);
            _FOVCoroutine = _fovControlComponent.StartCoroutine(AnimateFOV(targetFOV, duration));
            return _FOVCoroutine;
        }

        private IEnumerator AnimateFOV(float target, float time)
        {
            var start = _fovControlComponent.FOV;
            var elapsed = 0f;
            while (elapsed < time)
            {
                elapsed += Time.deltaTime;
                _fovControlComponent.FOV = Mathf.Lerp(start, target, elapsed / time);
                yield return null;
            }
            _fovControlComponent.FOV = target;
            _FOVCoroutine = null;
        }
    }
}
