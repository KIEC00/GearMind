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
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector3(0, Force, 0), ForceMode2D.Impulse);
        yield return new WaitForSeconds(CaldownAfter);
        IsCanPush = true;
    }

    public void OnTriggerStay2D(Collider2D collider)
    {
        if(IsCanPush && collider.gameObject.tag == "Instrument" )
        {
            StartCoroutine(PushObject(collider.gameObject));
        }
    }

    
}
