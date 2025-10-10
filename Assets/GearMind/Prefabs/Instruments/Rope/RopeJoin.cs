using UnityEngine;

public class RopeJoin : MonoBehaviour
{
    [SerializeField] private SpringJoint Sp;
    [SerializeField] private DrawRope DrawRope;
    public SpringJoint SecondSp {  get; private set; }
    public bool IsCanJoin { get; private set; } = true;

    public GameObject ArchorObject { get; private set; }

    public void OnTriggerEnter(Collider other)
    {
        if (IsCanJoin && other.gameObject.tag == "Instrument")
        {
            IsCanJoin = false;
            var rg = other.gameObject.GetComponent<Rigidbody>();
            Sp.connectedBody = rg;
            DrawRope.ChangeSecondObject(other.gameObject);

        }
    }
    

    public void Awake()
    {
        SecondSp = GetComponent<SpringJoint>();
        ArchorObject = this.gameObject;
    }
}
