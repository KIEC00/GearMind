using EditorAttributes;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class LaserBeam : MonoBehaviour
{
    [SerializeField, Required, OnValueChanged(nameof(InitLineRendererForce))]
    private LineRenderer _lineRenderer;

    [SerializeField, OnValueChanged(nameof(InitLineRendererWidth))]
    private float _laserWidth = 0.1f;

    public void UpdateBeam(Vector3 startPos, Vector3 endPos)
    {
        _lineRenderer.SetPosition(0, startPos);
        _lineRenderer.SetPosition(1, endPos);
    }

    private void OnValidate() => InitLineRenderer();

    [Button]
    private void InitLineRendererForce() => InitLineRenderer(true);

    private void InitLineRenderer(bool force = false)
    {
        if (!_lineRenderer)
            _lineRenderer = GetComponent<LineRenderer>();
        if (!_lineRenderer)
            return;
        if (force || _lineRenderer.positionCount != 2)
        {
            _lineRenderer.SetPositions(new Vector3[] { Vector3.zero, Vector3.zero });
            InitLineRendererWidth();
        }
    }

    private void InitLineRendererWidth()
    {
        _lineRenderer.startWidth = _laserWidth;
        _lineRenderer.endWidth = _laserWidth;
    }

    private void OnEnable() => _lineRenderer.enabled = true;

    private void OnDisable() => _lineRenderer.enabled = false;
}
