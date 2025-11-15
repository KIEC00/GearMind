using Assets.GearMind.Grid;
using UnityEngine;

namespace Assets.GearMind.Instruments
{
    public interface IDragAndDropTarget
    {
        bool IsDragable { get; set; }
        GridComponent Grid { get; set; }
        void OnDragStart();
        void OnDrag(Vector3 position);
        void OnDragEnd();
        void SetError(bool isError);
        bool ValidatePlacement();
    }
}
