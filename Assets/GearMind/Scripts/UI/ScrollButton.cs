using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class HorizontalScrollButton : MonoBehaviour, IPointerDownHandler
{
    private enum ScrollDirection { LEFT, RIGHT }

    [SerializeField] private ScrollDirection direction;
    [SerializeField] private float cellSize = 440f;
    [SerializeField] private float scrollDuration = 0.4f;
    [SerializeField] private ScrollRect scrollRect;

    private bool isScrolling = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        if (isScrolling) return;
        isScrolling = true;
        ScrollByOneCell();
    }

    private void ScrollByOneCell()
    {
        if (scrollRect.content == null) return;

        float contentWidth = scrollRect.content.rect.width;
        if (contentWidth <= 0) return;

        float normalizedStep = cellSize / contentWidth;
        float current = scrollRect.horizontalNormalizedPosition;
        float target = current + (direction == ScrollDirection.RIGHT ? -normalizedStep : normalizedStep);

        target = Mathf.Clamp01(target);

        if (Mathf.Approximately(target, current))
        {
            isScrolling = false;
            return;
        }

        StartCoroutine(SmoothScrollTo(target, scrollDuration));
    }

    private IEnumerator SmoothScrollTo(float target, float duration)
    {
        float start = scrollRect.horizontalNormalizedPosition;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            float t = elapsed / duration;
            scrollRect.horizontalNormalizedPosition = Mathf.Lerp(start, target, t);
            elapsed += Time.deltaTime;
            yield return null;
        }

        scrollRect.horizontalNormalizedPosition = target;
        isScrolling = false;
    }
}