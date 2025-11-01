using System.Collections;
using UnityEngine;

public class Spring : MonoBehaviour
{
    [SerializeField]
    private int Force = 2;

    [SerializeField]
    private float CaldownBefore = 0;

    [SerializeField]
    private float CaldownAfter = 0;
    public bool IsCanPush { get; private set; } = true;

    private IEnumerator PushObject(GameObject gameObject)
    {
        IsCanPush = false;
        yield return new WaitForSeconds(CaldownBefore);
        gameObject.GetComponent<Rigidbody2D>().AddForce(new Vector2(0, Force), ForceMode2D.Impulse);
        yield return new WaitForSeconds(CaldownAfter);
        IsCanPush = true;
    }

    public void OnTriggerEnter2D(Collider2D collider)
    {
        if (
            IsCanPush
            && (collider.gameObject.tag == "Instrument" || collider.gameObject.tag == "Ball")
        )
        {
            StartCoroutine(PushObject(collider.gameObject));
        }
    }
}
