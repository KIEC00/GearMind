using Assets.GearMind.Instruments;
using Assets.GearMind.State;
using Assets.GearMind.State.Utils;
using EditorAttributes;
using Newtonsoft.Json.Serialization;
using Unity.VisualScripting;
using UnityEngine;

public class PlaceObjectClass
    : MonoBehaviour,
        IDragAndDropTarget,
        IHaveState<Rigidbody2DState>,
        IGameplayObject
{
    private const float DRAG_ALPHA = 0.5f;

    [field: SerializeField]
    public bool IsDragable { get; set; } = false;

    [SerializeField, Required]
    private Renderer _renderer;

    private Color _initialColor;

    [SerializeField, Required]
    private Rigidbody2D _rigidbody;

    [SerializeField]
    private ContactFilter2D _contactFilter;

    public virtual void EnterEditMode()
    {
        throw new System.NotImplementedException();
    }

    public virtual void EnterPlayMode()
    {
        throw new System.NotImplementedException();
    }

    public virtual Rigidbody2DState GetState()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDrag(Vector3 position)
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDragEnd()
    {
        throw new System.NotImplementedException();
    }

    public virtual void OnDragStart()
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetError(bool isError)
    {
        throw new System.NotImplementedException();
    }

    public virtual void SetState(Rigidbody2DState state)
    {
        throw new System.NotImplementedException();
    }

    public virtual bool ValidatePlacement()
    {
        throw new System.NotImplementedException();
    }
}
