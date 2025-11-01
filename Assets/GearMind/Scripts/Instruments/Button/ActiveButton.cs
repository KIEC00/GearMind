using UnityEngine;

public class ActiveButton : MonoBehaviour
{
    [SerializeField] private GameObject ActiveCollider;

    public void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Instrument" || other.gameObject.tag == "Ball")
        {
            ActiveCollider.SetActive(true);
        }
    }
        
}
