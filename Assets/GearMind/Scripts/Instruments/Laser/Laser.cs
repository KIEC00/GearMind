using UnityEngine;

public class Laser : MonoBehaviour
{
    [SerializeField] private LineRenderer LineRenderer;
    [SerializeField] private int MaxDistance;
    [SerializeField] private Transform StartLaserPoint;
    [SerializeField] private float LaserWidth = 0.1f;
    public int LayerMask = 3;



    //добавить обработку столкновения с сыром(когда будет сыр)
    public void UpdateLaser()
    {

        var hit = Physics2D.Raycast(StartLaserPoint.position, transform.right, MaxDistance, 1 << LayerMask);
        if(hit.collider != null)
        {
            LineRenderer.SetPosition(0, StartLaserPoint.position);
            LineRenderer.SetPosition(1, hit.point);
        }
        else
        {
            LineRenderer.SetPosition(1, StartLaserPoint.position + transform.right * MaxDistance);
            LineRenderer.SetPosition(0, StartLaserPoint.position);

        }
    }

   


    public void Start()
    {
        LineRenderer.startWidth = LaserWidth;
        LineRenderer.endWidth = LaserWidth;
    }

    public void Update()
    {
        UpdateLaser();
    }
}
