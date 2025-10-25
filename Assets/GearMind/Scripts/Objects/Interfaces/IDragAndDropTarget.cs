using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Objects
{
    public interface IDragAndDropTarget
    {
        bool IsDragable { get; set; }
        void OnDragStart();
        void OnDragPerform(Vector3 position);
        void OnDragEnd();
        bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn);
    }
}
