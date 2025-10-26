using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class CommonGridObject : MonoBehaviour, IDragAndDropTarget
{
    [SerializeField] private LayerMask ObstacleLayers;
    [SerializeField] private Material PlaceMaterial;
    [SerializeField] private Material GridMaterial;
    private Collider2D[] ListCollidersCollisions = new Collider2D[10];
    private Collider2D ObjectCollider;
    private ContactFilter2D Filter;
    private Renderer ObjectRenderer;



    public void Awake()
    {

        ObjectCollider = GetComponent<Collider2D>();
        Filter = new ContactFilter2D();
        Filter.SetLayerMask(ObstacleLayers);
        Filter.useTriggers = false;
        ObjectRenderer = GetComponent<Renderer>();

        OnDragStart();
    }

    public void DestroyObject()
    {
        Destroy(this);
    }

    public void OnDragEnd()
    {
        ObjectCollider.isTrigger = false;
        ObjectRenderer.material = PlaceMaterial;
    }

    public void OnDragStart()
    {
        ObjectCollider.isTrigger = true;
        ObjectRenderer.material = GridMaterial;
    }

    public bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn)
    {
        dependsOn = new List<IDragAndDropTarget>();
        return ObjectCollider.Overlap(Filter, ListCollidersCollisions) == 0;
    }

}

