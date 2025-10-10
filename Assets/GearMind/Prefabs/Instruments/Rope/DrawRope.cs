using System.Collections;
using UnityEngine;

public class DrawRope : MonoBehaviour
{
    [SerializeField] private Transform FirstObjectPoint;
    [SerializeField] private Transform SecondObjectPoint;
    [SerializeField] private LineRenderer LineRenderer;

    public void ChangeSecondObject(GameObject gameObject)
    {
        SecondObjectPoint = gameObject.transform;
    }

    public void Start()
    {
        LineRenderer.SetPosition(1, SecondObjectPoint.position);
        LineRenderer.SetPosition(0, FirstObjectPoint.position);
    }

    public void Update()
    {
        LineRenderer.SetPosition(1, SecondObjectPoint.position);
    }

}
