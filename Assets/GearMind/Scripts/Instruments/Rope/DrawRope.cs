using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

public class DrawRope : MonoBehaviour
{
    [SerializeField] private Transform FirstObjectPoint;
    [SerializeField] private Transform SecondObjectPoint;
    [SerializeField] private LineRenderer LineRenderer;
    [SerializeField] private float OffsetDrawRopeZ;
    private Vector3 OffsetDrawRopVector;

    public void ChangeSecondObject(GameObject gameObject)
    {
        SecondObjectPoint = gameObject.transform;
    }

    public void Awake()
    {
        OffsetDrawRopVector = new Vector3 (0, 0, OffsetDrawRopeZ);
    }


    public void Start()
    {
        LineRenderer.SetPosition(1, SecondObjectPoint.position + OffsetDrawRopVector);
        LineRenderer.SetPosition(0, FirstObjectPoint.position + OffsetDrawRopVector);
    }

    public void Update()
    {
        LineRenderer.SetPosition(0, FirstObjectPoint.position + OffsetDrawRopVector);
        LineRenderer.SetPosition(1, SecondObjectPoint.position + OffsetDrawRopVector);
    }

}
