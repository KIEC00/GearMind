using System;
using Assets.GearMind.Level;
using VContainer;

namespace Assets.GearMind.UI
{
    public class UIManager
    {
        public bool IsEditMode => _isEditMode;
        public bool IsSimulateMode => !_isEditMode;
        public event Action<bool> OnModeChanged;
        
        private bool _isEditMode = true;
        private LevelStateMachine _stateMachine;

        [Inject]
        public void Construct(LevelStateMachine stateMachine)
        {
            _stateMachine = stateMachine;
        }

        public void EnterEditMode()
        {
            if (!_isEditMode)
            {
                _isEditMode = true;
                _stateMachine.TransitionTo(LevelState.Edit);
                OnModeChanged?.Invoke(true);
            }
        }

        public void EnterSimulateMode()
        {
            if (_isEditMode)
            {
                _isEditMode = false;
                _stateMachine.TransitionTo(LevelState.Simulate);
                OnModeChanged?.Invoke(false);
            }
        }

        public void ToggleMode()
        {
            if (_isEditMode)
                EnterSimulateMode();
            else
                EnterEditMode();
        }
    }
}