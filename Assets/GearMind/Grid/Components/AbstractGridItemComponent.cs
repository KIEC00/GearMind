using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    public abstract class AbstractGridItemComponent : MonoBehaviour
    {
        public abstract IEnumerable<Cell> Cells { get; }
    }
}
