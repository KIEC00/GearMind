using System.Collections.Generic;
using Assets.GearMind.Grid;
using Assets.GearMind.Instruments;
using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.VFX;

public class CommonGridObject
    : MonoBehaviour,
        IDragAndDropTarget,
        IHaveState<Rigidbody2DState>,
        IGameplayObject
{
    [SerializeField]
    private LayerMask ObstacleLayers;

    [SerializeField]
    private Material GridMaterial;

    [SerializeField]
    private bool IsNeedTrigerCollider;

    private Collider2D[] ListCollidersCollisions = new Collider2D[10];

    [SerializeField, Required]
    private Collider2D ObjectCollider;
    private ContactFilter2D Filter;

    [SerializeField, Required]
    private Renderer ObjectRenderer;
    private RigidbodyType2D TypeObjectRigidbody;

    [SerializeField, Required]
    private Rigidbody2D Rigidbody;
    private const float DRAG_ALPHA = 0.5f;
    private Color InitialColor;
    private Material InitialMaterial;

    [field: SerializeField]
    public bool IsDragable { get; set; } = false;
    public GridComponent Grid { get; set; }

    public void Awake()
    {
        Filter = new ContactFilter2D();
        Filter.SetLayerMask(ObstacleLayers);
        Filter.useTriggers = true;
        InitialColor = ObjectRenderer.material.color;
        InitialMaterial = ObjectRenderer.material;
        TypeObjectRigidbody = Rigidbody.bodyType;

        EnterEditMode();
    }

    public void DestroyObject()
    {
        Destroy(this);
    }

    //public bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn)
    //{
    //    dependsOn = new List<IDragAndDropTarget>();
    //    return ObjectCollider.Overlap(Filter, ListCollidersCollisions) == 0;
    //}

    public bool ValidatePlacement()
    {
        return ObjectCollider.Overlap(Filter, ListCollidersCollisions) == 0;
    }

    public virtual Rigidbody2DState GetState() => Rigidbody.GetState();

    public virtual void SetState(Rigidbody2DState state) => Rigidbody.SetState(state);

    [Button]
    public virtual void EnterEditMode() => Rigidbody.bodyType = RigidbodyType2D.Static;

    [Button]
    public virtual void EnterPlayMode()
    {
        ObjectCollider.isTrigger = IsNeedTrigerCollider;
        Rigidbody.bodyType = TypeObjectRigidbody;
    }

    public void OnDrag(Vector3 position) => this.transform.position = position;

    public void SetError(bool isError)
    {
        ObjectRenderer.material.color = isError
            ? Color.red.WithAlpha(DRAG_ALPHA)
            : InitialColor.WithAlpha(DRAG_ALPHA);
    }

    public void OnDragEnd()
    {
        ObjectRenderer.material = InitialMaterial;
    }

    public void OnDragStart()
    {
        ObjectCollider.isTrigger = true;
        ObjectRenderer.material = GridMaterial;
    }
}
