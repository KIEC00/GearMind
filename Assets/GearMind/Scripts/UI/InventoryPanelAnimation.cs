using System.Collections;
using UnityEngine;

namespace Assets.GearMind.UI
{
    public class InventoryPanelAnimation : MonoBehaviour
    {
        [SerializeField]
        private float _slideDuration = 0.3f;

        private RectTransform _rectTransform;
        private Vector2 _shownPosition;
        private Vector2 _hiddenPosition;
        private Coroutine _slideCoroutine;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _shownPosition = _rectTransform.anchoredPosition;
            _hiddenPosition = _shownPosition - new Vector2(0, _rectTransform.rect.height);
        }

        public void Slide(bool hide)
        {
            if (_slideCoroutine != null)
                StopCoroutine(_slideCoroutine);

            _slideCoroutine = StartCoroutine(SlidePanel(hide));
        }

        private IEnumerator SlidePanel(bool hide)
        {
            var targetPosition = hide ? _hiddenPosition : _shownPosition;
            var startPosition = _rectTransform.anchoredPosition;
            var startAnimationTime = 0f;

            while (startAnimationTime < _slideDuration)
            {
                startAnimationTime += Time.deltaTime;
                var t = startAnimationTime / _slideDuration;
                t = 1 - Mathf.Pow(1 - t, 3);
                _rectTransform.anchoredPosition = Vector2.Lerp(startPosition, targetPosition, t);
                yield return null;
            }

            _rectTransform.anchoredPosition = targetPosition;
            _slideCoroutine = null;
        }
    }
    
}