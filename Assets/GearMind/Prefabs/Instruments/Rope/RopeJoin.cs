using System.Collections;
using UnityEngine;

public class RopeJoin : MonoBehaviour
{
    [SerializeField] private GameObject HeadRopeObject;
    [SerializeField] private DrawRope DrawRope;
    [SerializeField] private BreakRope BreakRope;
    public SpringJoint Sp {  get; private set; }
    public bool IsCanJoin { get; private set; } = true;


    public void OnTriggerEnter(Collider other)
    {
        if (IsCanJoin && other.gameObject.tag == "Instrument")
        {
            IsCanJoin = false;
            var rg = other.gameObject.GetComponent<Rigidbody>();
            Sp.connectedBody = rg;
            DrawRope.ChangeSecondObject(other.gameObject);
            BreakRope.ActiveClick();
            

        }
    }

    
    public void BreakJoint()
    {
        
        Sp.spring = 0;
        Sp.damper = 0;
        var rb = HeadRopeObject.GetComponent<Rigidbody>();
        rb.useGravity = true;
        rb.isKinematic = false;
        Destroy(HeadRopeObject, 0.5f);
        Destroy(this.gameObject, 0.5f);
    }

    public void Start()
    {
        Sp = HeadRopeObject.GetComponent<SpringJoint>();
    }



}
