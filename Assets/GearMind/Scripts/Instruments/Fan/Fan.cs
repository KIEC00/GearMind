using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : MonoBehaviour, IIncludedObject
{
    [SerializeField]
    private GameObject VentilatorEffect;

    [SerializeField]
    private float ForceVentilator = 20;

    [SerializeField]
    private Renderer _renderer;

    private Color _initialColor;

    private void Awake() => _initialColor = _renderer.material.color;

    public void TurnOnOff(bool isTurnOn)
    {
        VentilatorEffect.SetActive(isTurnOn);
        if (isTurnOn)
            _renderer.material.color = Color.green;
        else
            _renderer.material.color = _initialColor;
    }

    public void RotateVentilator()
    {
        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void VentilatorPush(Rigidbody2D rb)
    {
        rb.AddForce(-transform.right * ForceVentilator, ForceMode2D.Force);
    }

    private void OnEnable()
    {
        TurnOnOff(false);
    }

    private void OnDisable()
    {
        TurnOnOff(false);
    }
}
