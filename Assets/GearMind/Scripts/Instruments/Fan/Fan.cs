using UnityEngine;
using UnityEngine.EventSystems;

public class Fan : MonoBehaviour, IPointerClickHandler, IIncludedObject
{
    public bool IsTurnOn { get; private set; } = false;
    [SerializeField] private GameObject VentilatorEffect;
    [SerializeField] private float ForceVentilator = 10;

    [SerializeField] private Material TurnOnMaterial;
    [SerializeField] private Material TurnOffMaterial;

    public void TurnOnOff(bool isTurnOn)
    {
        VentilatorEffect.SetActive(isTurnOn);
        IsTurnOn = !IsTurnOn;
        if (isTurnOn)
            gameObject.GetComponent<Renderer>().material = TurnOnMaterial;
        else
            gameObject.GetComponent<Renderer>().material = TurnOffMaterial;
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
