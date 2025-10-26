using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public interface IDragAndDropTarget 
{
    public bool ValidatePlacement(out IEnumerable<IDragAndDropTarget> dependsOn);
    public void DestroyObject();
    public void OnDragStart();

    public void OnDragEnd();
}
