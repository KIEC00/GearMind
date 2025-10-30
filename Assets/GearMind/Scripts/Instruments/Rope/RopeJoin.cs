using System.Collections;
using UnityEngine;

public class RopeJoin : MonoBehaviour
{
    [SerializeField] private GameObject HeadRopeObject;
    [SerializeField] private DrawRope DrawRope;
    [SerializeField] private BreakRope BreakRope;
    public SpringJoint2D Sp {  get; private set; }
    public bool IsCanJoin { get; private set; } = true;
    


    public void OnTriggerEnter2D(Collider2D other)
    {
        if (IsCanJoin && other.gameObject.tag == "Instrument")
        {
            IsCanJoin = false;
            var rg = other.gameObject.GetComponent<Rigidbody2D>();
            Sp.connectedBody = rg;
            DrawRope.ChangeSecondObject(other.gameObject);
            BreakRope.ActiveClick();
            

        }
    }

    
    public void BreakJoint()
    {
        
        Sp.frequency = 0;
        Sp.dampingRatio = 0;
        var rb = HeadRopeObject.GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
        rb.gravityScale = 1;
        Destroy(HeadRopeObject, 0.5f);
        Destroy(this.gameObject, 0.5f);
    }

    public void Start()
    {
        Sp = HeadRopeObject.GetComponent<SpringJoint2D>();
    }



}
