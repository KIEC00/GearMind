using Assets.GearMind.Objects;
using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using Assets.Utils.Runtime;
using EditorAttributes;
using UnityEngine;
using UnityEngine.UIElements;

public class CommonGridObject : MonoBehaviour, IDragAndDropTarget, IHaveState<Rigidbody2DState>, IGameplayObject
{
    [SerializeField] private LayerMask ObstacleLayers;
    [SerializeField] private bool IsNeedTrigerCollider;
    [SerializeField] private Texture2D WhiteTexture;

    private Collider2D[] ListCollidersCollisions = new Collider2D[10];
    private Collider2D ObjectCollider;
    private ContactFilter2D Filter;
    private Renderer ObjectRenderer;
    private RigidbodyType2D TypeObjectRigidbody;
    private Rigidbody2D Rigidbody;


    private const float DRAG_ALPHA = 0.5f;
    private Color InitialColor;
    private Texture2D InitialTexture;
    private Material InitialMaterial;


    [field: SerializeField]
    public bool IsDragable { get; set; } = false;

    public void Awake()
    {

        ObjectCollider = GetComponent<Collider2D>();
        Rigidbody = GetComponent<Rigidbody2D>();
        Filter = new ContactFilter2D();
        Filter.SetLayerMask(ObstacleLayers);
        Filter.useTriggers = true;

        ObjectRenderer = GetComponent<Renderer>();
        if (ObjectRenderer == null)
        {
            ObjectRenderer = GetComponentInChildren<Renderer>(true);
        }
        InitialMaterial = ObjectRenderer.sharedMaterial;
        InitialColor = InitialMaterial.GetColor("_BaseColor");
        InitialTexture = InitialMaterial.GetTexture("_BaseMap") as Texture2D;
        TypeObjectRigidbody = Rigidbody.bodyType;

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
    public virtual void EnterEditMode() => Rigidbody.bodyType = RigidbodyType2D.Kinematic;

    [Button]
    public virtual void EnterPlayMode()
    {
        ObjectCollider.isTrigger = IsNeedTrigerCollider;
        Rigidbody.bodyType = TypeObjectRigidbody;
    }

    public void OnDrag(Vector3 position) => this.transform.position = position;

    public void SetError(bool isError)
    {
        if (ObjectRenderer?.material == null) return;

        Material mat = ObjectRenderer.material;

        if (isError)
        {
            if (WhiteTexture != null)
            {
                mat.SetTexture("_BaseMap", WhiteTexture);
            }
            mat.SetColor("_BaseColor", new Color(0.7f, 0.1f, 0.1f, DRAG_ALPHA));
        }
        else
        {
            if (InitialTexture != null)
            {
                mat.SetTexture("_BaseMap", InitialTexture);
            }
            mat.SetColor("_BaseColor", InitialColor.WithAlpha(DRAG_ALPHA));
        }
    }


    public void OnDragEnd()
    {
        ObjectCollider.isTrigger = IsNeedTrigerCollider;

        if (ObjectRenderer.material != InitialMaterial)
        {
            Destroy(ObjectRenderer.material);
        }
        ObjectRenderer.sharedMaterial = InitialMaterial;
    }

    public void OnDragStart()
    {
        if (ObjectRenderer == null) return;

        ObjectCollider.isTrigger = true;
        Material dragMat = Instantiate(InitialMaterial);
        ObjectRenderer.material = dragMat;
        SetError(false);
    }
}
