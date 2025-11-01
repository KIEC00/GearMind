using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : MonoBehaviour, IPointerClickHandler, IIncludedObject
{
    public bool IsTurnOn { get; private set; } = false;

    [SerializeField]
    private GameObject VentilatorEffect;

    [SerializeField]
    private float ForceVentilator = 20;

    [SerializeField]
    private Renderer _renderer;

    private Color _initialColor;

    public void TurnOnOff(bool isTurnOn)
    {
        VentilatorEffect.SetActive(isTurnOn);
        IsTurnOn = isTurnOn;
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
        rb.AddForce(-transform.right * ForceVentilator);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        TurnOnOff(!IsTurnOn);
    }
}
