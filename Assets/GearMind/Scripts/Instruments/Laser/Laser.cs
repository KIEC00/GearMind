using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer LineRenderer;
    [SerializeField] private int MaxDistance;
    [SerializeField] private Transform StartLaserPoint;
    [SerializeField] private float OffsetDrawLaser;
    [SerializeField] private float LaserWidth = 0.1f;
    public int LayerMask = 3;
    private Vector3 OffsetDrawLaserVector;



    //добавить обработку столкновения с сыром(когда будет сыр)
    public void UpdateLaser()
    {

        var hit = Physics2D.Raycast(StartLaserPoint.position, transform.right, MaxDistance, 1 << LayerMask);
        if(hit.collider != null)
        {
            LineRenderer.SetPosition(0, StartLaserPoint.position + OffsetDrawLaserVector);
            LineRenderer.SetPosition(1, new Vector3(hit.point.x, hit.point.y, OffsetDrawLaser));
        }
        else
        {
            LineRenderer.SetPosition(1, StartLaserPoint.position + transform.right * MaxDistance + OffsetDrawLaserVector);
            LineRenderer.SetPosition(0, StartLaserPoint.position + OffsetDrawLaserVector);

        }
    }

   


    public void Start()
    {
        LineRenderer.startWidth = LaserWidth;
        LineRenderer.endWidth = LaserWidth;
    }

    public void Awake()
    {
        OffsetDrawLaserVector = new Vector3(0,0,OffsetDrawLaser);
    }

    public void Update()
    {
        UpdateLaser();
    }
}
