using System;

namespace Assets.GearMind.UI
{
    public class UIManager
    {
        public bool IsEditMode => _isEditMode;
        public bool IsSimulateMode => !_isEditMode;
        public event Action<bool> OnModeChanged;
        public event Action OnLevelPassed;
        private bool _isEditMode = true;

        public void EnterEditMode()
        {
            if (!_isEditMode)
            {
                _isEditMode = true;
                OnModeChanged?.Invoke(true);
            }
        }

        public void EnterSimulateMode()
        {
            if (_isEditMode)
            {
                _isEditMode = false;
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

        public void OpenNextLevelMenu() => OnLevelPassed?.Invoke();
    }
}
