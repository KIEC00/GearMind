using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.Events;

public class ButtonInstrument : MonoBehaviour
{

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "IncludedInstrument")
        {
            var includedObject = collision.gameObject.GetComponent<IncludedObject>();
            includedObject.TurnOnOff(true);
        }
    }



    public void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "IncludedInstrument")
        {
            var includedObject = collision.gameObject.GetComponent<IncludedObject>();
            includedObject.TurnOnOff(false);
        }
    }
}
