using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public interface IDragAndDropTarget
    {
        bool IsDragable { get; set; }
        void OnDragStart();
        void OnDrag(Vector3 position);
        void OnDragEnd();
        void SetError(bool isError);
        bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn);
    }
}
