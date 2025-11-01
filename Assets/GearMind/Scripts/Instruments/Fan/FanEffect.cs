using R3.Triggers;
using UnityEngine;

public class FanEffect : MonoBehaviour
{
    [SerializeField] private Fan Ventilator;

    

    public void OnTriggerStay2D(Collider2D other)
    {
        
        if(other.gameObject.tag == "Instrument" || other.gameObject.tag == "Ball")
        {
            Ventilator.VentilatorPush(other.GetComponent<Rigidbody2D>());
        }
    }
    public void OnTriggerEnter2D(Collider2D other)
    {

        if (other.gameObject.tag == "Instrument" || other.gameObject.tag == "Ball")
        {
            Ventilator.VentilatorPush(other.GetComponent<Rigidbody2D>());
        }
    }


}
