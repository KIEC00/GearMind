using System.Collections;
using System.Collections.Generic;
using Assets.GearMind.Objects;
using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;

public class JointGridObject : MonoBehaviour, IDragAndDropTarget, IHaveState<Rigidbody2DState>, IGameplayObject
{
    [SerializeField] private LayerMask ConnectLayers;
    [SerializeField] private LayerMask ObstacleLayers;
    [SerializeField] private Material GridMaterial;
    [SerializeField] private bool IsNeedTrigerCollider;
    [SerializeField] private GameObject LogicObject;
    private Collider2D[] ListCollidersCollisions = new Collider2D[10];
    private Collider2D ObjectCollider;
    private ContactFilter2D Filter;
    private ContactFilter2D ConnectFilter;

    private Rigidbody2D Rigidbody;
    private const float DRAG_ALPHA = 0.5f;
    private Color InitialColor;
    private Material InitialMaterial;

    private List<IConnectGridObject> ConnectObjects;
    [SerializeField]private Renderer ObjectRenderer;

    private IEnumerable<IDragAndDropTarget> GridObjects;

    [SerializeField, Required]
    private Rigidbody2D _rigidbody;

    [field: SerializeField]
    public bool IsDragable { get; set; } = false;


    public void Awake()
    {
        ObjectCollider = GetComponent<Collider2D>();
        
        Filter = new ContactFilter2D();
        Filter.SetLayerMask(ObstacleLayers);
        Filter.useTriggers = true;

        ConnectFilter = new ContactFilter2D();
        ConnectFilter.SetLayerMask(ConnectLayers);
        ConnectFilter.useTriggers = true;

        ConnectObjects = new List<IConnectGridObject>();
        //ObjectRenderer = GetComponent<Renderer>();

        InitialMaterial = ObjectRenderer.material;
        InitialColor = ObjectRenderer.material.color;  

        OnDragStart();
    }


    //Добавить логику удаления;
    public void DestroyObject()
    {
        foreach (var connectObject in ConnectObjects)
        {
            connectObject.OnDestroyConnectObject -= DestroyObject;
        }
        Destroy(gameObject);
    }

    public bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn)
    {
        
        if (ObjectCollider.Overlap(Filter, ListCollidersCollisions) == 0)
        {
            var countCollision = ObjectCollider.Overlap(ConnectFilter, ListCollidersCollisions);
            var listDepends = new List<IDragAndDropTarget>();
            if (countCollision > 0)
            {
                for(var i = 0; i < countCollision; i++)
                {
                    listDepends.Add(ListCollidersCollisions[i].transform.parent.GetComponent<IDragAndDropTarget>());
                }
                dependsOn = listDepends;
                return true;
            }
        }
        dependsOn = new List<IDragAndDropTarget>();
        return false;
    }

    public bool ValidatePlacement()
    {

        if (ObjectCollider.Overlap(Filter, ListCollidersCollisions) == 0)
        {
            var countCollision = ObjectCollider.Overlap(ConnectFilter, ListCollidersCollisions);
            var listDepends = new List<IDragAndDropTarget>();
            if (countCollision > 0)
            {
                for (var i = 0; i < countCollision; i++)
                {
                    listDepends.Add(ListCollidersCollisions[i].transform.parent.GetComponent<IDragAndDropTarget>());
                }               
                return true;
            }
        }
        return false;
    }

    public void RegisterConnect()
    {
        var countCollision = ObjectCollider.Overlap(ConnectFilter, ListCollidersCollisions);
        if (countCollision > 0)
        {
            for (var i = 0; i < countCollision; ++i)
            {
                var connectObject = ListCollidersCollisions[i].gameObject.GetComponent<IConnectGridObject>();
                connectObject.OnDestroyConnectObject += DestroyObject;
                ConnectObjects.Add(connectObject);

            }
        }
        
    }

    public void ClearConnects()
    {
        foreach(var obj in ConnectObjects)
        {
            obj.OnDestroyConnectObject -= DestroyObject;
        }
    }

    public void SetError(bool isError)
    {
        ObjectRenderer.material.color = isError
            ? Color.red.WithAlpha(DRAG_ALPHA)
            : InitialColor.WithAlpha(DRAG_ALPHA);
    }

    public void OnDragEnd()
    {
        if(!IsNeedTrigerCollider)
            ObjectCollider.isTrigger = false;
        ObjectRenderer.material = InitialMaterial;
        RegisterConnect();
    }

    public void OnDragStart()
    {
        ObjectCollider.isTrigger = true;
        ObjectRenderer.material = GridMaterial;
        ClearConnects();
    }

    public void OnDrag(Vector3 position) => transform.position = position;


    public virtual Rigidbody2DState GetState() => _rigidbody.GetState();
    public virtual void SetState(Rigidbody2DState state) => _rigidbody.SetState(state);

    [Button]
    public void EnterEditMode() => LogicObject.SetActive(false);

    [Button]
    public void EnterPlayMode() => LogicObject.SetActive(true);
    
}

