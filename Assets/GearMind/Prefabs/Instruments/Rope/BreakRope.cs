using UnityEngine;
using UnityEngine.EventSystems;

public class BreakRope : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private RopeJoin RopeJoin;
    public bool IsCanClick { get; private set; } = false;
    public void OnPointerClick(PointerEventData eventData)
    {
        if(IsCanClick)
        {
            RopeJoin.BreakJoint();
        }
    }

    public void ActiveClick()
    {
        IsCanClick = true;
    }
}
