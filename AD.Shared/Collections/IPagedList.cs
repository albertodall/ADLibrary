using System.Collections.Generic;

namespace AD.Shared.Collections
{
    /// <summary>
    /// Represents a subset of a collection of objects that can be individually accessed by index and containing metadata about the superset collection of objects this subset was created from.
    /// </summary>
    /// <typeparam name="T">The type of object the collection should contain.</typeparam>
    /// <seealso cref="IList{T}"/>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// Total number of subsets within the superset.
        /// </summary>
        int PageCount { get; }

        /// <summary>
        /// Total number of objects contained within the superset.
        /// </summary>
        int TotalItemCount { get; }

        /// <summary>
        /// Zero-based index of this subset within the superset.
        /// </summary>
        int PageIndex { get; }

        /// <summary>
        /// One-based index of this subset within the superset.
        /// </summary>
        int PageNumber { get; }

        /// <summary>
        /// Maximum size any individual subset.
        /// </summary>
        int PageSize { get; }

        /// <summary>
        /// Returns true if this is NOT the first subset within the superset.
        /// </summary>
        bool HasPreviousPage { get; }

        /// <summary>
        /// Returns true if this is NOT the last subset within the superset.
        /// </summary>
        bool HasNextPage { get; }

        /// <summary>
        /// Returns true if this is the first subset within the superset.
        /// </summary>
        bool IsFirstPage { get; }

        /// <summary>
        /// Returns true if this is the last subset within the superset.
        /// </summary>
        bool IsLastPage { get; }
    }
}
