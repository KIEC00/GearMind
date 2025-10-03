using UnityEngine;
using UnityEngine.UI;

namespace Assets.GearMind.Scripts.UI
{
    public abstract class PopupBinder<T> : WindowBinder<T> where T : WindowViewModel
    {
        [SerializeField] 
        private Button _btnClose;

        protected virtual void Start()
        {
            _btnClose?.onClick.AddListener(OnCloseButtonClick);
        }

        protected virtual void OnDestroy()
        {
            _btnClose?.onClick.RemoveListener(OnCloseButtonClick);
        }

        protected virtual void OnCloseButtonClick()
        {
            ViewModel.RequestClose();
        }
    }
}
