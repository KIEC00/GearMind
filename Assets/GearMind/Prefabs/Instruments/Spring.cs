using Assets.GearMind.Objects;
using EditorAttributes;
using UnityEngine;

public class Spring : MonoBehaviour,IGameplayObject, INotConnectedObject
{
    [Header("Force")]
    [SerializeField]
    private float _forseSpring;

    [Header("")]
    [SerializeField, Required]
    private Collider2D _pushCollider;

    [SerializeField, Required]
    private AreaEffector2D _areaEffector;

    [SerializeField, Required]
    private Collider2D _collider;




    public  void EnterEditMode()
    {
        _collider.enabled = true;
        _pushCollider.enabled = false;
    }

    public  void EnterPlayMode()
    {
        _collider.enabled = false;
        _pushCollider.enabled = true;   
    }

    public void Awake()
    {
        _areaEffector.forceMagnitude = _forseSpring;
    }
}
