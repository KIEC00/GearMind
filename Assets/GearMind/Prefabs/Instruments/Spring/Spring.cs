using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField] private int Force = 13;
    [SerializeField] private float CaldownBefore = 1;
    [SerializeField] private float CaldownAfter = 2;
    public bool IsCanPush { get; private set; } = true;

    private IEnumerator PushObject(GameObject gameObject)
    {
        
        IsCanPush = false;
        yield return new WaitForSeconds(CaldownBefore);
        gameObject.GetComponent<Rigidbody>().AddForce(new Vector3(0, Force, 0), ForceMode.Impulse);
        yield return new WaitForSeconds(CaldownAfter);
        IsCanPush = true;
    }

    public void OnCollisionStay(Collision collision)
    {
        if(IsCanPush && collision.gameObject.tag == "Instrument" )
        {
            StartCoroutine(PushObject(collision.gameObject));
        }
    }

    
}
