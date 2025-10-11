using R3.Triggers;
using UnityEngine;

public class VentilatorEffect : MonoBehaviour
{
    [SerializeField] private Ventilator Ventilator;

    

    public void OnTriggerStay(Collider other)
    {
        
        if(other.gameObject.tag == "Instrument")
        {
            Ventilator.VentilatorPush(other.GetComponent<Rigidbody>());
        }
    }

    
}
