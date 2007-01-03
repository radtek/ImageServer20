using System;
using System.Collections.Generic;
using System.Text;

namespace ClearCanvas.Desktop.Tables
{
    /// <summary>
    /// Defines the interface to a table, which provides a presentation model for viewing data in a tabular form.
    /// </summary>
    public interface ITable
    {
        /// <summary>
        /// Returns the <see cref="Type"/> of the items in this table.
        /// </summary>
        Type ItemType { get; }

        /// <summary>
        /// Gets the collection of items
        /// </summary>
        IItemCollection Items { get; }

        /// <summary>
        /// Get the collection of columns
        /// </summary>
        ITableColumnCollection Columns { get; }

        /// <summary>
        /// Sorts this table according to the cached sort parameters, if any exist.
        /// </summary>
        void Sort();

        /// <summary>
        /// Sorts this table according to the specified sort parameters.
        /// </summary>
        /// <param name="sortParams"></param>
        void Sort(TableSortParams sortParams);

        /// <summary>
        /// Gets the cached sort parameters, or returns null if this table has not been sorted.
        /// </summary>
        TableSortParams SortParams { get; }

        /// <summary>
        /// Gets the base column width for this table, in units that correspond roughly to the
        /// width of one character.
        /// </summary>
        float BaseColumnWidthChars { get; }
    }
}
