using Assets.GearMind.Level;
using UnityEngine;
using VContainer;

namespace Assets.GearMind.UI
{
    public class UIManager
    {
        public bool IsEditMode => _isEditMode;
        public bool IsSimulateMode => !_isEditMode;
        
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
            }
        }

        public void EnterSimulateMode()
        {
            if (_isEditMode)
            {
                _isEditMode = false;
                _stateMachine.TransitionTo(LevelState.Simulate);
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