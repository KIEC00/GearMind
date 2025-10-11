using System.Collections.Generic;
using UnityEngine;

namespace Assets.GearMind.Grid.Components
{
    /// <summary>
    /// Defines the contract for objects that can be placed and managed within a grid system.
    /// Implement this interface to create grid-aware objects that can be validated, placed, and removed from a grid.
    /// </summary>
    public interface IGridObject
    {
        /// <summary>
        /// Gets the collection of primary grid cells that this object currently occupies.
        /// These are the main cells where the object is physically located.
        /// </summary>
        /// <value>An enumerable collection of Cell objects representing the occupied cells.</value>
        IEnumerable<Cell> Cells { get; }

        /// <summary>
        /// Retrieves additional cells associated with this object at the specified position,
        /// such as trigger zones, influence areas, or validation regions.
        /// </summary>
        /// <param name="position">The grid position to check for additional cells.</param>
        /// <param name="grid">The grid context in which the object is being evaluated.</param>
        /// <returns>
        /// An enumerable collection of additional cells, or null if no additional cells exist.
        /// These cells are typically used for validation triggers or special effects.
        /// </returns>
        /// <remarks>
        /// For objects that are already placed in the grid, this method MUST return consistent results
        /// regardless of changes in surrounding cells or grid state.
        /// </remarks>
        public IEnumerable<Cell> GetAdditionalCellsAt(Vector2Int position, Grid grid);

        /// <summary>
        /// Validates whether this object can be placed at the specified position within the given grid.
        /// This method may be invoked multiple times, including when the object is already placed.
        /// </summary>
        /// <param name="position">The target position to validate for placement.</param>
        /// <param name="grid">The grid context to validate against.</param>
        /// <returns>
        /// <c>true</c> if the object can be placed at the specified position; otherwise, <c>false</c>.
        /// </returns>
        /// <remarks>
        /// Implementations should handle the case where the object is validating its current position
        /// and should account for its own presence during validation.
        /// </remarks>
        bool ValidateAt(Vector2Int position, Grid grid);

        /// <summary>
        /// Called after the object has been successfully placed in the grid.
        /// Use this method to perform post-placement setup such as establishing relationships,
        /// setting transform parents, or initializing not grid-based dependencies.
        /// </summary>
        /// <param name="position">The position where the object was placed.</param>
        /// <param name="grid">The grid in which the object was placed.</param>
        void AfterPlace(Vector2Int position, Grid grid);

        /// <summary>
        /// Called before the object is removed from the grid.
        /// Use this method to perform cleanup operations such as breaking relationships,
        /// removing event subscriptions, or cleaning up not grid-based dependencies.
        /// </summary>
        /// <param name="grid">The grid from which the object is being removed.</param>
        void BeforeRemove(Grid grid);
    }
}
