using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JointGridObject : MonoBehaviour, IDragAndDropTarget
{
    [SerializeField] private LayerMask ConnectLayers;
    [SerializeField] private LayerMask ObstacleLayers;
    [SerializeField] private Material PlaceMaterial;
    [SerializeField] private Material GridMaterial;
    private Collider2D[] ListCollidersCollisions = new Collider2D[10];
    private Collider2D ObjectCollider;
    private ContactFilter2D Filter;
    private ContactFilter2D ConnectFilter;

    private List<IConnectGridObject> ConnectObjects;
    private Renderer ObjectRenderer;

    private IEnumerable<IDragAndDropTarget> GridObjects;


    public void Awake()
    {
        ObjectCollider = GetComponent<Collider2D>();
        
        Filter = new ContactFilter2D();
        Filter.SetLayerMask(ObstacleLayers);
        Filter.useTriggers = false;

        ConnectFilter = new ContactFilter2D();
        ConnectFilter.SetLayerMask(ConnectLayers);
        ConnectFilter.useTriggers = true;

        ConnectObjects = new List<IConnectGridObject>();
        ObjectRenderer = GetComponent<Renderer>();

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

    public void OnDragEnd()
    {
        ObjectCollider.isTrigger = false;
        ObjectRenderer.material = PlaceMaterial;
        RegisterConnect();
    }

    public void OnDragStart()
    {
        ObjectCollider.isTrigger = true;
        ObjectRenderer.material = GridMaterial;
        ClearConnects();
    }

    public void LogValidate()
    {
        Debug.Log(ValidatePlacement(out GridObjects));

    }

    
}

